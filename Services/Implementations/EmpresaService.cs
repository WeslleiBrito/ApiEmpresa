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


            // verificar se já existe empresa com o mesmo CNPJ

            var empresaExistente = (await _repo.GetAllAsync()).FirstOrDefault(e => e.Cnpj == dto.Cnpj);

            if (empresaExistente != null)
            {
                throw new ValidationException(
                    "Cnpj",
                    "Já existe uma empresa cadastrada com este CNPJ."
                );
            }

            await _repo.AddAsync(empresa);
            await _repo.SaveChangesAsync();

            // recarregar dados já com Setor incluso
            var empresaCompleta = await _repo.GetEmpresaCompletaAsync(empresa.Id);

            return _mapper.Map<EmpresaResponseDTO>(empresaCompleta);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var empresa = await _repo.GetEmpresaCompletaAsync(id) ?? throw new NotFoundException("Empresa não encontrada.");

            _repo.Delete(empresa);
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<EmpresaResponseDTO?> PatchAsync(Guid empresaId, PatchEmpresaDTO dto)
        {
            var empresa = await _repo.GetEmpresaCompletaAsync(empresaId) ?? throw new NotFoundException("Empresa não encontrada.");

            // Atualiza SOMENTE o que veio no DTO
            _mapper.Map(dto, empresa);

            await _repo.SaveChangesAsync();

            var empresaAtualizada = await _repo.GetEmpresaCompletaAsync(empresaId);
            return _mapper.Map<EmpresaResponseDTO>(empresaAtualizada);
        }

        public async Task<EmpresaResponseDTO> AddSetoresAsync(Guid empresaId, AddSetorDTO dto)
        {
            var empresa = await _repo.GetEmpresaCompletaAsync(empresaId)
                ?? throw new NotFoundException("Empresa não encontrada.");

            // Setores já vinculados
            var setoresEmpresaIds = empresa.Setores
                .Select(s => s.SetorId)
                .ToHashSet();

            // Filtrar setores novos
            var setoresParaAdicionar = dto.SetoresIds
                .Where(id => !setoresEmpresaIds.Contains(id))
                .Distinct()
                .ToList();

            if (!setoresParaAdicionar.Any())
                return _mapper.Map<EmpresaResponseDTO>(empresa);

            // Validar existência dos setores
            var setoresExistentes = await _setorRepo.GetByIdsAsync(setoresParaAdicionar);

            if (setoresExistentes.Count() != setoresParaAdicionar.Count)
            {
                var existentesIds = setoresExistentes.Select(s => s.Id).ToHashSet();
                var setoresInvalidos = setoresParaAdicionar
                    .Where(id => !existentesIds.Contains(id))
                    .ToList();

                throw new NotFoundException(
                    "Um ou mais setores informados não existem.",
                    setoresInvalidos.Cast<object>()
                );
            }

            // Persistir vínculo
            foreach (var setorId in setoresParaAdicionar)
            {
                await _repo.AddSetorAsync(empresaId, setorId);
            }

            await _repo.SaveChangesAsync();

            var empresaAtualizada = await _repo.GetByIdWithFullDataAsNoTrackingAsync(empresaId);

            return _mapper.Map<EmpresaResponseDTO>(empresaAtualizada);
        }

        public async Task RemoveSetoresAsync(Guid empresaId, RemoveSetorDTO dto)
        {
            foreach (var setorId in dto.SetoresIds)
            {
                await _repo.RemoveSetorAsync(empresaId, setorId);
            }

            await _repo.SaveChangesAsync();
        }

        public async Task<EmpresaResponseDTO> AlocarFuncionarioSetorAsync(Guid empresaId, AddFuncionarioEmpresaDTO dto)
        {
            var empresa = await _repo.GetEmpresaCompletaAsync(empresaId) ?? throw new NotFoundException("Empresa não encontrada.");

            var setoresEmpresaIds = empresa.Setores.Select(s => s.SetorId).ToHashSet();

            List<Guid> setoresInvalidos = [.. dto.Funcionarios
                .Where(item => !setoresEmpresaIds.Contains(item.SetorId))
                .Select(item => item.SetorId)];

            if (setoresInvalidos.Count != 0)
            {
                throw new NotFoundException(
                    "Um ou mais setores informados não estão vinculados à empresa.",
                    setoresInvalidos.Cast<object>()
                );
            }

            var funcionarios = (await _funcionarioRepo.FindAsync([.. dto.Funcionarios.Select(f => f.FuncionarioId)])).ToList();

            if (funcionarios.Count < dto.Funcionarios.Count)
            {
                var funcionariosExistentesIds = funcionarios.Select(f => f.Id).ToHashSet();

                var funcionariosInvalidos = dto.Funcionarios
                    .Where(item => !funcionariosExistentesIds.Contains(item.FuncionarioId))
                    .Select(item => item.FuncionarioId)
                    .ToList();

                if (funcionariosInvalidos.Count != 0)
                {
                    throw new NotFoundException(
                        "Um ou mais funcionários informados não existem.",
                        funcionariosInvalidos.Cast<object>()
                    );
                }
            }


            foreach (var func in dto.Funcionarios)
            {

                await _repo.AddFuncionarioAsync(
                    empresa.Id,
                    func.SetorId,
                    func.FuncionarioId,
                    func.Salario
                );
            }


            await _repo.SaveChangesAsync();

            var empresaAtualizada = await _repo.GetByIdWithFullDataAsNoTrackingAsync(empresaId);

            return _mapper.Map<EmpresaResponseDTO>(empresaAtualizada);
        }

        public async Task<EmpresaResponseDTO> RemoveFuncionarioAsync(Guid empresaId, RemoveFuncionarioEmpresaDTO dto)
        {
            var empresa = await _repo.GetEmpresaCompletaAsync(empresaId)
                ?? throw new NotFoundException("Empresa não encontrada.");

            // Setores válidos da empresa
            var setoresEmpresa = empresa.Setores
                .Select(s => s.SetorId)
                .ToHashSet();

            // Validação de setores inválidos
            var setoresInvalidos = dto.Funcionarios
                .Where(f => !setoresEmpresa.Contains(f.SetorId))
                .Select(f => f.SetorId)
                .Distinct()
                .ToList();

            if (setoresInvalidos.Any())
            {
                throw new NotFoundException(
                    "Um ou mais setores informados não pertencem à empresa.",
                    setoresInvalidos.Cast<object>()
                );
            }

            // Remover vínculos
            foreach (var item in dto.Funcionarios)
            {
                await _repo.RemoveFuncionarioAsync(
                    empresaId,
                    item.SetorId,
                    item.FuncionarioId
                );
            }

            await _repo.SaveChangesAsync();

            var empresaAtualizada =
                await _repo.GetByIdWithFullDataAsNoTrackingAsync(empresaId);

            return _mapper.Map<EmpresaResponseDTO>(empresaAtualizada);
        }

    }
}


