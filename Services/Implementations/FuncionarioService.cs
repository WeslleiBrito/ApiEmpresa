using ApiEmpresas.DTOs.Funcionario;
using ApiEmpresas.Exceptions;
using ApiEmpresas.Models;
using ApiEmpresas.Repositories.Interfaces;
using ApiEmpresas.Services.Interfaces;
using AutoMapper;

namespace ApiEmpresas.Services.Implementations
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _repo;
        private readonly IEmpresaRepository _empresaRepo;
        private readonly IProfissaoRepository _profissaoRepo;
        private readonly IHabilidadeRepository _habilidadeRepo;
        private readonly IMapper _mapper;

        public FuncionarioService(
            IFuncionarioRepository repo,
            IEmpresaRepository empresaRepo,
            IProfissaoRepository profissaoRepo,
            ISetorRepository setorRepo,
            IHabilidadeRepository habilidadeRepo,
            IMapper mapper)
        {
            _repo = repo;
            _empresaRepo = empresaRepo;
            _profissaoRepo = profissaoRepo;
            _habilidadeRepo = habilidadeRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FuncionarioResponseDTO>> GetAllAsync()
        {
            var funcionarios = await _repo.GetAllCompletoAsync();
            return _mapper.Map<IEnumerable<FuncionarioResponseDTO>>(funcionarios);
        }

        public async Task<FuncionarioResponseDTO?> GetByIdAsync(Guid id)
        {
            var funcionario = await _repo.GetFuncionarioCompletoAsync(id) ?? throw new NotFoundException("O funcionário não foi encontrado.");

            return _mapper.Map<FuncionarioResponseDTO>(funcionario);
        }

        public async Task<FuncionarioResponseDTO> CreateAsync(CreateFuncionarioDTO dto)
        {
            // 1. Validar CPF único
            if (await _repo.ExisteCpfAsync(dto.Cpf)) throw new ConflictException("Já existe um funcionário com esse CPF.");

            // 2. Validar se a Empresa existe
            var empresa = await _empresaRepo.GetByIdWithFullDataAsync(dto.EmpresaId) ?? throw new NotFoundException("A empresa informada não foi encontrada.");

            // 3. Validar se a Profissão existe
            var profissao = await _profissaoRepo.GetByIdAsync(dto.ProfissaoId) ?? throw new NotFoundException("A profissão informada não foi encontrada.");

            // 4. Validar se os Setores existem e pertencem à Empresa
            List<Guid> setoresIdEmpresa = [.. empresa.Setores.Select(es => es.SetorId)];

            // 5. Verificar se algum dos setores informados não pertence à empresa
            List<Guid> setoresInvalidos = dto.SetoresId.FindAll((id) => !setoresIdEmpresa.Contains(id));

            if (setoresInvalidos.Count != 0)
            {
                throw new NotFoundException(
                    "Setor(es) informados não foram encontrados para a empresa selecionada.",
                    setoresInvalidos.Cast<object>()
                );
            }

            // 6. Mapear e Criar
            var funcionario = _mapper.Map<Funcionario>(dto);

            funcionario.Setores = [];

            // 7. Adicionamos os itens na lista
            foreach (var setorId in dto.SetoresId)
            {
                funcionario.Setores.Add(new FuncionarioSetor
                {
                    SetorId = setorId
                });
            }

            if (dto.Telefone != null)
            {
                funcionario.Telefone = dto.Telefone;
            }

            // 8. Persistir

            await _repo.AddAsync(funcionario);
            await _repo.SaveChangesAsync();

            // Retorna o DTO mapeado
            var funcionarioCriado = await _repo.GetFuncionarioCompletoAsync(funcionario.Id);

            return _mapper.Map<FuncionarioResponseDTO>(funcionarioCriado);
        }

        public async Task<FuncionarioResponseDTO> AddSetoresAsync(Guid funcionarioId, AddSetorDTO dto)
        {
            // Recupera funcionário completo
            var funcionario = await _repo.GetFuncionarioCompletoAsync(funcionarioId)
                ?? throw new NotFoundException("Funcionário não encontrado.");

            Guid empresaId = funcionario.EmpresaId;

            // Recupera empresa para validar setores
            var empresa = await _empresaRepo.GetByIdWithFullDataAsync(empresaId)
                ?? throw new NotFoundException("A empresa do funcionário não foi encontrada.");

            // Setores existentes na empresa
            var setoresEmpresaIds = empresa.Setores.Select(s => s.SetorId).ToHashSet();

            // Valida setores informados
            var setoresInvalidos = dto.SetoresIds
                .Where(id => !setoresEmpresaIds.Contains(id))
                .ToList();

            if (setoresInvalidos.Count > 0)
            {
                throw new NotFoundException(
                    "Um ou mais setores informados não existem na empresa do funcionário.",
                    setoresInvalidos.Cast<object>()
                );
            }

            // Setores atuais do funcionário
            var setoresAtuais = funcionario.Setores.Select(fs => fs.SetorId).ToHashSet();

            // Descobrir novos setores a vincular
            var novosVinculos = dto.SetoresIds
                .Where(id => !setoresAtuais.Contains(id))
                .Select(id => new FuncionarioSetor
                {
                    FuncionarioId = funcionarioId,
                    SetorId = id
                })
                .ToList();

            // Adicionar novos vínculos
            foreach (var vinculo in novosVinculos)
            {
                funcionario.Setores.Add(vinculo);
            }

            await _repo.SaveChangesAsync();

            // Retornar resultado atualizado
            var atualizado = await _repo.GetFuncionarioCompletoAsync(funcionarioId);
            return _mapper.Map<FuncionarioResponseDTO>(atualizado);
        }

        public async Task<FuncionarioResponseDTO> RemoveSetorAsync(Guid funcionarioId, Guid setorId)
        {
            var funcionario = await _repo.GetFuncionarioCompletoAsync(funcionarioId)
                ?? throw new NotFoundException("Funcionário não encontrado.");

            var vinculo = funcionario.Setores.FirstOrDefault(s => s.SetorId == setorId)
                ?? throw new ValidationException("setorId", "Este setor não está vinculado ao funcionário.");

            funcionario.Setores.Remove(vinculo);

            await _repo.SaveChangesAsync();

            var atualizado = await _repo.GetFuncionarioCompletoAsync(funcionarioId);
            return _mapper.Map<FuncionarioResponseDTO>(atualizado);
        }

        public async Task<FuncionarioResponseDTO> UpdateAsync(Guid id, UpdateFuncionarioDTO dto)
        {
            // -----------------------------------------------------------------------
            // 1. RECUPERAR O FUNCIONÁRIO (COM OS RELACIONAMENTOS)
            // -----------------------------------------------------------------------
            // Precisamos do funcionário completo (com Setores e Empresa) para validar as trocas.
            var funcionario = await _repo.GetFuncionarioCompletoAsync(id) ?? throw new NotFoundException("Funcionário não encontrado.");

            // Regra: Não pode zerar os setores
            if (dto.SetoresId.Count == 0)
            {
                throw new ValidationException("SetoresId", "O funcionário deve pertencer a pelo menos um setor.");
            }

            // Regra: O funcionário deve estar ligado a setores da empresa ATUAL dele.
            // Como o DTO de Update NÃO tem o campo EmpresaId, assumimos que ele continua na mesma empresa.
            // Se ele mudasse de empresa, teríamos que validar os setores da NOVA empresa.

            Guid idEmpresaAtual = funcionario.EmpresaId;

            // Buscamos a empresa para garantir que os setores novos pertencem a ela
            var empresa = await _empresaRepo.GetByIdWithFullDataAsync(idEmpresaAtual) ?? throw new NotFoundException("A empresa vinculada a este funcionário não foi encontrada.");

            // Extraímos os IDs dos setores válidos dessa empresa
            List<Guid> idsSetoresDaEmpresa = empresa.Setores.Select(es => es.SetorId).ToList();

            // Verificamos se algum ID enviado no DTO NÃO está na lista da empresa
            var setoresInvalidos = dto.SetoresId.Where(id => !idsSetoresDaEmpresa.Contains(id)).ToList();

            if (setoresInvalidos.Count > 0)
                throw new NotFoundException(
                    "Setor(es) informados não foram encontrados para a empresa selecionada.",
                    setoresInvalidos.Cast<object>()
                );
            {
            }

            // -----------------------------------------------------------------------
            // 3. ATUALIZAÇÃO DOS CAMPOS SIMPLES
            // -----------------------------------------------------------------------

            funcionario.Nome = dto.Nome;
            funcionario.Salario = dto.Salario;
            funcionario.Telefone = dto.Telefone; // Pode ser nulo, tudo bem.

            // Atualiza Profissão (Se mudou)
            if (funcionario.ProfissaoId != dto.ProfissaoId)
            {
                if (!await _profissaoRepo.ExistsAsync(dto.ProfissaoId))
                    throw new NotFoundException("A profissão informada não foi encontrada.");

                funcionario.ProfissaoId = dto.ProfissaoId;
            }

            // Atualiza Endereço (Objeto de Valor)
            // Assumimos que o EF Core rastreia a mudança nas propriedades internas
            funcionario.Endereco.Logradouro = dto.Endereco.Logradouro;
            funcionario.Endereco.Numero = dto.Endereco.Numero;
            funcionario.Endereco.Complemento = dto.Endereco.Complemento;
            funcionario.Endereco.Bairro = dto.Endereco.Bairro;
            funcionario.Endereco.Cidade = dto.Endereco.Cidade;
            funcionario.Endereco.Estado = dto.Endereco.Estado;
            funcionario.Endereco.Cep = dto.Endereco.Cep;

            // -----------------------------------------------------------------------
            // 4. ATUALIZAÇÃO DO RELACIONAMENTO MUITOS-PARA-MUITOS (SETORES)
            // -----------------------------------------------------------------------
            // O EF Core precisa saber quem SAIU e quem ENTROU.
            // Não podemos fazer apenas "funcionario.Setores = novaLista", pois perde o tracking.

            // A. Identificar quem deve ser REMOVIDO (está no banco, mas não no DTO)
            // Usamos ToList() para materializar e evitar erro de modificação da coleção durante o loop
            var setoresParaRemover = funcionario.Setores
                .Where(fs => !dto.SetoresId.Contains(fs.SetorId))
                .ToList();

            foreach (var setorRemovido in setoresParaRemover)
            {
                funcionario.Setores.Remove(setorRemovido);
            }

            // B. Identificar quem deve ser ADICIONADO (está no DTO, mas não no banco)
            var idsAtuais = funcionario.Setores.Select(fs => fs.SetorId).ToList();
            var novosIds = dto.SetoresId.Where(id => !idsAtuais.Contains(id)).ToList();

            foreach (var novoId in novosIds)
            {
                funcionario.Setores.Add(new FuncionarioSetor
                {
                    SetorId = novoId
                    // FuncionarioId é preenchido automaticamente pelo EF
                });
            }

            // -----------------------------------------------------------------------
            // 5. PERSISTÊNCIA E RETORNO
            // -----------------------------------------------------------------------

            await _repo.SaveChangesAsync();

            // Retorna o objeto atualizado mapeado para DTO
            return _mapper.Map<FuncionarioResponseDTO>(funcionario);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var funcionario = await _repo.GetByIdAsync(id) ?? throw new NotFoundException("O funcionário não foi encontrado.");

            _repo.Delete(funcionario);
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<FuncionarioResponseDTO> AddHabilidadesAsync(
    Guid funcionarioId,
    AddHabilidadesFuncionarioDTO dto)
{
    var funcionario = await _repo.GetFuncionarioCompletoAsync(funcionarioId)
        ?? throw new NotFoundException("Funcionário não encontrado.");

    // 1. Validar se as habilidades existem
    var habilidadesExistentes = await _habilidadeRepo.GetByIdsAsync(dto.HabilidadesIds);
    var habilidadesExistentesIds = habilidadesExistentes.Select(h => h.Id).ToHashSet();

    var habilidadesInvalidas = dto.HabilidadesIds
        .Where(id => !habilidadesExistentesIds.Contains(id))
        .ToList();

    if (habilidadesInvalidas.Any())
    {
        throw new NotFoundException(
            "Habilidade(s) não encontrada(s).",
            habilidadesInvalidas.Cast<object>()
        );
    }

    // 2. Descobrir quais já existem no funcionário
    var atuais = funcionario.Habilidades
        .Select(fh => fh.HabilidadeId)
        .ToHashSet();

    var novosVinculos = dto.HabilidadesIds
        .Where(id => !atuais.Contains(id))
        .Select(id => new FuncionarioHabilidade
        {
            FuncionarioId = funcionarioId,
            HabilidadeId = id
        })
        .ToList();

    if (novosVinculos.Any())
        await _repo.AddFuncionarioHabilidadesAsync(novosVinculos);

    await _repo.SaveChangesAsync();

    var atualizado = await _repo.GetFuncionarioCompletoAsNoTrackingAsync(funcionarioId);
    return _mapper.Map<FuncionarioResponseDTO>(atualizado);
}

    }
}