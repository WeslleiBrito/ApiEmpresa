using ApiEmpresas.DTOs.Empresa;
using ApiEmpresas.Exceptions;
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
            var empresa = await _repo.GetEmpresaCompletaAsync(id) ?? throw new NotFoundException("Empresa não encontrada.");

            return _mapper.Map<EmpresaResponseDTO>(empresa);
        }

        public async Task<EmpresaResponseDTO> CreateAsync(CreateEmpresaDTO dto)
        {
            // mapear
            var empresa = _mapper.Map<Empresa>(dto);

            if (dto.SetoresIds != null && dto.SetoresIds.Count != 0)
            {
                var setoresExistentes = await _setorRepo.GetByIdsAsync(dto.SetoresIds);
                var setoresExistentesIds = setoresExistentes.Select(s => s.Id).ToHashSet();

                var setoresInvalidos = dto.SetoresIds
                    .Where(id => !setoresExistentesIds.Contains(id))
                    .ToList();

                if (setoresInvalidos.Count != 0)
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
            var empresa = await _repo.GetEmpresaCompletaAsync(id) ?? throw new NotFoundException("Empresa não encontrada.");

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

        public async Task<EmpresaResponseDTO?> PatchAsync(Guid id, PatchEmpresaDTO dto)
        {
            var empresa = await _repo.GetEmpresaCompletaAsync(id) ?? throw new NotFoundException("Empresa não encontrada.");

            // Atualiza SOMENTE o que veio no DTO
            _mapper.Map(dto, empresa);

            await _repo.SaveChangesAsync();

            var updated = await _repo.GetByIdWithFullDataAsNoTrackingAsync(id);
            return _mapper.Map<EmpresaResponseDTO>(updated);
        }

        public async Task<EmpresaResponseDTO> AddSetoresAsync(Guid id, AddSetorDTO dto)
        {
            var empresa = await _repo.GetEmpresaCompletaAsync(id) ?? throw new NotFoundException("Empresa não encontrada.");

            Guid empresaId = empresa.Id;

            // Valida se todos os setores existem

            var setoresExistentes = await _setorRepo.GetByIdsAsync(dto.SetoresIds);
            var setoresExistentesIds = setoresExistentes.Select(s => s.Id).ToHashSet();

            var setoresInvalidos = dto.SetoresIds
                .Where(id => !setoresExistentesIds.Contains(id))
                .ToList();

            if (setoresInvalidos.Count != 0)
            {
                throw new NotFoundException(
                    "Um ou mais setores informados não existem.",
                    setoresInvalidos.Cast<object>()
                );
            }


            // 3. Descobrir o que já existe
            var atuais = empresa.Setores.Select(s => s.SetorId).ToHashSet();

            var novosVinculos = dto.SetoresIds
                .Where(id => !atuais.Contains(id))
                .Select(id => new EmpresaSetor
                {
                    EmpresaId = empresaId,
                    SetorId = id
                })
                .ToList();


            // 4. Adicionar usando o repositório
            await _repo.AddEmpresaSetoresAsync(novosVinculos);
            await _repo.SaveChangesAsync();

            empresa = await _repo.GetByIdWithFullDataAsNoTrackingAsync(id);
            return _mapper.Map<EmpresaResponseDTO>(empresa);
        }

        public async Task<EmpresaResponseDTO> RemoveSetorAsync(Guid empresaId, Guid setorId)
        {
            var empresa = await _repo.GetEmpresaCompletaAsync(empresaId)
                ?? throw new NotFoundException("Empresa não encontrada.");

            // Verifica se o setor existe
            var setorExiste = await _setorRepo.ExistsAsync(setorId);
            if (!setorExiste)
                throw new NotFoundException("O setor informado não existe.");

            // Verifica se o setor está vinculado à empresa
            var vinculo = empresa.Setores.FirstOrDefault(s => s.SetorId == setorId) ?? throw new ValidationException("setorId", "Este setor não está vinculado à empresa.");

            // Remove vínculo
            await _repo.RemoveEmpresaSetorAsync(vinculo);
            await _repo.SaveChangesAsync();

            // Retorna empresa atualizada
            var empresaAtualizada = await _repo.GetByIdWithFullDataAsNoTrackingAsync(empresaId);

            return _mapper.Map<EmpresaResponseDTO>(empresaAtualizada);
        }

    }
}


