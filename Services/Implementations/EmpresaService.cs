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


        public async Task<EmpresaResponseDTO> UpdateAsync(Guid id, UpdateEmpresaDTO dto)
        {
            // 1. Carregar a empresa do banco (Rastreada pelo EF)
            // Isso permite atualizar Nome, CNPJ e Endereço automaticamente
            var empresa = await _repo.GetEmpresaCompletaAsync(id) ?? throw new NotFoundException("Empresa não encontrada.");

            // 2. Validações
            if (await _repo.CnpjExisteParaOutraEmpresaAsync(dto.Cnpj, id))
                throw new ConflictException("O CNPJ informado já está em uso por outra empresa.");

            var setoresExistentes = await _setorRepo.GetByIdsAsync(dto.SetoresIds);

            IEnumerable<object> setoresNaoEncontrados = [];

            foreach (var setorId in dto.SetoresIds)
            {
                if (!setoresExistentes.Any(s => s.Id == setorId))
                {
                    setoresNaoEncontrados = setoresNaoEncontrados.Append(setorId);
                }
            }

            if (setoresNaoEncontrados.Any())
                throw new NotFoundException("Setor", setoresNaoEncontrados);

            if (dto.Endereco == null)
                throw new ValidationException("endereco", "O endereço é obrigatório.");

            // 3. Atualizar Propriedades Simples (Change Tracking cuida disso)
            empresa.Nome = dto.Nome;
            empresa.Cnpj = dto.Cnpj;
            empresa.RegimeTributario = dto.RegimeTributario;

            // 4. Atualizar Endereço (Change Tracking cuida disso pois é Owned Type)
            empresa.Endereco.Logradouro = dto.Endereco.Logradouro;
            empresa.Endereco.Numero = dto.Endereco.Numero;
            empresa.Endereco.Complemento = dto.Endereco.Complemento;
            empresa.Endereco.Bairro = dto.Endereco.Bairro;
            empresa.Endereco.Cidade = dto.Endereco.Cidade;
            empresa.Endereco.Estado = dto.Endereco.Estado;
            empresa.Endereco.Cep = dto.Endereco.Cep;

            // 5. ATUALIZAR SETORES (Lógica corrigida com inserção explícita)

            // Identifica os IDs
            var setoresAtuaisIds = empresa.Setores.Select(s => s.SetorId).ToHashSet();
            var setoresNovosIds = dto.SetoresIds.ToHashSet();

            // 5.1 Remover vínculos antigos
            var setoresParaRemover = empresa.Setores
                .Where(es => !setoresNovosIds.Contains(es.SetorId))
                .ToList();

            if (setoresParaRemover.Any())
            {
                // Remove explicitamente usando o método do repositório
                _repo.RemoveEmpresaSetores(setoresParaRemover);
            }

            // 5.2 Adicionar novos vínculos
            var idsParaAdicionar = setoresNovosIds
                .Where(id => !setoresAtuaisIds.Contains(id))
                .ToList();

            if (idsParaAdicionar.Any())
            {
                // Cria a lista de objetos EmpresaSetor
                var novosVinculos = idsParaAdicionar.Select(setorId => new EmpresaSetor
                {
                    EmpresaId = empresa.Id,
                    SetorId = setorId
                }).ToList();

                await _repo.AddEmpresaSetoresAsync(novosVinculos);
            }

            // 6. Persistir Tudo
            // Salva: Updates da Empresa, dados comuns e endereço
            await _repo.SaveChangesAsync();

            // 7. Recarregar dados para Retorno
            var empresaAtualizada = await _repo.GetByIdWithFullDataAsNoTrackingAsync(id);

            return _mapper.Map<EmpresaResponseDTO>(empresaAtualizada);
        }

    }
}


