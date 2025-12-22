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
   
        private readonly IHabilidadeRepository _habilidadeRepo;
        private readonly IMapper _mapper;

        public FuncionarioService(
            IFuncionarioRepository repo,
            IHabilidadeRepository habilidadeRepo,
            IMapper mapper)
        {
            _repo = repo;
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


            // 2. Mapear e Criar
            var funcionario = _mapper.Map<Funcionario>(dto);

            funcionario.FuncionarioSetorVinculo = [];

            if (dto.Telefone != null)
            {
                funcionario.Telefone = dto.Telefone;
            }

            // 4. Persistir
            await _repo.AddAsync(funcionario);
            await _repo.SaveChangesAsync();

            // 5. Retorna o DTO mapeado
            var funcionarioCriado = await _repo.GetFuncionarioCompletoAsync(funcionario.Id);

            return _mapper.Map<FuncionarioResponseDTO>(funcionarioCriado);
        }

        public async Task<FuncionarioResponseDTO> UpdateAsync(Guid id, UpdateFuncionarioDTO dto)
        {
            // -----------------------------------------------------------------------
            // 1. RECUPERAR O FUNCIONÁRIO (COM OS RELACIONAMENTOS)
            // -----------------------------------------------------------------------
            // Precisamos do funcionário completo (com Setores e Empresa) para validar as trocas.
            var funcionario = await _repo.GetFuncionarioCompletoAsync(id) ?? throw new NotFoundException("Funcionário não encontrado.");


            funcionario.Nome = dto.Nome;
            funcionario.Telefone = dto.Telefone;

            // Atualiza Endereço (Objeto de Valor)
            // Assumimos que o EF Core rastreia a mudança nas propriedades internas
            funcionario.Endereco.Logradouro = dto.Endereco.Logradouro;
            funcionario.Endereco.Numero = dto.Endereco.Numero;
            funcionario.Endereco.Complemento = dto.Endereco.Complemento;
            funcionario.Endereco.Bairro = dto.Endereco.Bairro;
            funcionario.Endereco.Cidade = dto.Endereco.Cidade;
            funcionario.Endereco.Estado = dto.Endereco.Estado;
            funcionario.Endereco.Cep = dto.Endereco.Cep;

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

        public async Task<FuncionarioResponseDTO> AddHabilidadesAsync( Guid funcionarioId, AddHabilidadesFuncionarioDTO dto)
        {
            var funcionario = await _repo.GetFuncionarioCompletoAsync(funcionarioId)
                ?? throw new NotFoundException("Funcionário não encontrado.");

            // 1. Validar se as habilidades existem
            var habilidadesExistentes = await _habilidadeRepo.GetByIdsAsync(dto.HabilidadesIds);
            var habilidadesExistentesIds = habilidadesExistentes.Select(h => h.Id).ToHashSet();

            var habilidadesInvalidas = dto.HabilidadesIds
                .Where(id => !habilidadesExistentesIds.Contains(id))
                .ToList();

            if (habilidadesInvalidas.Count != 0)
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

            var atualizado = await _repo.GetFuncionarioCompletoAsync(funcionarioId);
            return _mapper.Map<FuncionarioResponseDTO>(atualizado);
        }

        public async Task<FuncionarioResponseDTO> RemoveHabilidadesAsync(Guid funcionarioId, RemoveHabilidadesFuncionarioDTO dto)
        {
            var funcionario = await _repo.GetFuncionarioCompletoAsync(funcionarioId)
                ?? throw new NotFoundException("Funcionário não encontrado.");

            foreach (Guid habilidadeId in dto.HabilidadesIds)
            {
                var vinculo = funcionario.Habilidades.FirstOrDefault(fh => fh.HabilidadeId == habilidadeId);
                if (vinculo != null)
                {
                    funcionario.Habilidades.Remove(vinculo);
                }
            }

            await _repo.SaveChangesAsync();

            var atualizado = await _repo.GetFuncionarioCompletoAsync(funcionarioId);
            return _mapper.Map<FuncionarioResponseDTO>(atualizado);
        }

    }
}