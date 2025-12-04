using ApiEmpresas.DTOs.Empresa;
using ApiEmpresas.Models;
using ApiEmpresas.Repositories.Interfaces;
using ApiEmpresas.Services.Interfaces;
using AutoMapper;

namespace ApiEmpresas.Services.Implementations
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepository _repo;
        private readonly ISetorRepository _setorRepo;
        private readonly IMapper _mapper;
        private readonly IFuncionarioRepository _funcionarioRepo;

        public EmpresaService(
            IEmpresaRepository repo,
            ISetorRepository setorRepo,
            IMapper mapper,
            IFuncionarioRepository funcionarioRepo)
        {
            _repo = repo;
            _setorRepo = setorRepo;
            _mapper = mapper;
            _funcionarioRepo = funcionarioRepo;
        }

        public async Task<IEnumerable<EmpresaResponseDTO>> GetAllAsync()
        {
            var empresas = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<EmpresaResponseDTO>>(empresas);
        }

        public async Task<EmpresaResponseDTO?> GetByIdAsync(Guid id)
        {
            var empresa = await _repo.GetEmpresaCompletaAsync(id);
            if (empresa == null)
                return null;

            return _mapper.Map<EmpresaResponseDTO>(empresa);
        }

        public async Task<EmpresaResponseDTO> CreateAsync(CreateEmpresaDTO dto)
        {
            // mapear
            var empresa = _mapper.Map<Empresa>(dto);

            if (dto.SetoresIds != null && dto.SetoresIds.Any())
            {
                var setoresExistentes = await _setorRepo.GetByIdsAsync(dto.SetoresIds);
                var setoresExistentesIds = setoresExistentes.Select(s => s.Id).ToHashSet();

                var setoresInvalidos = dto.SetoresIds
                    .Where(id => !setoresExistentesIds.Contains(id))
                    .ToList();

                if (setoresInvalidos.Any())
                {
                    throw new ValidationException(
                        "SetoresIds",
                        $"{string.Join(", ", setoresInvalidos)}"
                    );
                }
            }
            else
            {
                dto.SetoresIds = [];
            }

            // verificar se já existe empresa com o mesmo CNPJ

            var empresaExistente = (await _repo.GetAllAsync()).FirstOrDefault(e => e.Cnpj == dto.Cnpj);

            if (empresaExistente != null)
            {
                throw new ValidationException(
                    "Cnpj",
                    "Já existe uma empresa cadastrada com este CNPJ."
                );
            }
            // criar vínculos empresa-setor
            empresa.Setores = dto.SetoresIds
                .Select(setorId => new EmpresaSetor
                {
                    SetorId = setorId
                }).ToList();

            await _repo.AddAsync(empresa);
            await _repo.SaveChangesAsync();

            // recarregar dados já com Setor incluso
            var empresaCompleta = await _repo.GetEmpresaCompletaAsync(empresa.Id);

            return _mapper.Map<EmpresaResponseDTO>(empresaCompleta);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var empresa = await _repo.GetByIdAsync(id);

            if (empresa == null)
                return false;

            // Verificar se há funcionários vinculados
            bool possuiFuncionarios = await _funcionarioRepo.ExisteFuncionarioPorEmpresaAsync(id);

            if (possuiFuncionarios)
                return false; // ou lançar exceção se preferir

            // Remover vínculos com Setores
            var setores = await _repo.GetEmpresaSetoresAsync(id);
            if (setores.Any())
                _repo.RemoveEmpresaSetores(setores);

            _repo.Delete(empresa);
            await _repo.SaveChangesAsync();

            return true;
        }


        public async Task<EmpresaResponseDTO> UpdateAsync(Guid id, UpdateEmpresaDTO dto)
        {
            var empresa = await _repo.GetEmpresaCompletaAsync(id)
                ?? throw new ValidationException("id", $"Empresa não encontrada para o ID [{id}].");

            // 1. Validar setores
            var setoresExistentes = await _setorRepo.GetByIdsAsync(dto.SetoresIds);
            var existentesIds = setoresExistentes.Select(s => s.Id).ToHashSet();

            var setoresInvalidos = dto.SetoresIds.Where(id => !existentesIds.Contains(id)).ToList();
            if (setoresInvalidos.Any())
                throw new ValidationException("SetoresIds", $"Setores inexistentes: {string.Join(", ", setoresInvalidos)}");

            // 2. Verificar setores que serão REMOVIDOS
            var setoresAtuais = empresa.Setores.Select(s => s.SetorId).ToHashSet();
            var setoresParaRemover = setoresAtuais.Except(dto.SetoresIds).ToList();

            if (setoresParaRemover.Any())
            {
                foreach (var setorId in setoresParaRemover)
                {
                    bool possuiFuncionarios = await _funcionarioRepo.ExisteFuncionarioPorSetorAsync(setorId);
                    if (possuiFuncionarios)
                        throw new ValidationException("SetoresIds", $"Não é possível remover o setor {setorId} pois há funcionários vinculados.");
                }
            }

            // 3. Atualizar dados simples
            _mapper.Map(dto, empresa);

            // 4. Atualizar setores (limpa e recria)
            empresa.Setores.Clear();
            empresa.Setores = dto.SetoresIds
                .Select(id => new EmpresaSetor { SetorId = id })
                .ToList();

            _repo.Update(empresa);
            await _repo.SaveChangesAsync();

            var empresaAtualizada = await _repo.GetEmpresaCompletaAsync(empresa.Id);
            return _mapper.Map<EmpresaResponseDTO>(empresaAtualizada);
        }


    }
}


