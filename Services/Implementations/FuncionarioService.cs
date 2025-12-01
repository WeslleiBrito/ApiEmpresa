using ApiEmpresas.DTOs.Funcionario;
using ApiEmpresas.Models;
using ApiEmpresas.Repositories.Interfaces;
using ApiEmpresas.Services.Interfaces;
using AutoMapper;


namespace ApiEmpresas.Services.Implementations
{
    public class FuncionarioService : IFuncionarioService
{
    private readonly IFuncionarioRepository _repository;
    private readonly IMapper _mapper;

    public FuncionarioService(IFuncionarioRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FuncionarioResponseDTO>> GetAllAsync()
    {
        var funcionarios = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<FuncionarioResponseDTO>>(funcionarios);
    }

    public async Task<FuncionarioResponseDTO?> GetByIdAsync(Guid id)
    {
        var funcionario = await _repository.GetByIdAsync(id);
        return funcionario == null ? null : _mapper.Map<FuncionarioResponseDTO>(funcionario);
    }

    public async Task<FuncionarioResponseDTO> CreateAsync(CreateFuncionarioDTO dto)
    {
        var funcionario = _mapper.Map<Funcionario>(dto);
        await _repository.AddAsync(funcionario);
        await _repository.SaveChangesAsync();

        return _mapper.Map<FuncionarioResponseDTO>(funcionario);
    }

    public async Task<FuncionarioResponseDTO> UpdateAsync(Guid id, UpdateFuncionarioDTO dto)
    {
        var funcionario = await _repository.GetByIdAsync(id)
            ?? throw new Exception("Funcionário não encontrado.");

        _mapper.Map(dto, funcionario);
        _repository.Update(funcionario);

        await _repository.SaveChangesAsync();
        return _mapper.Map<FuncionarioResponseDTO>(funcionario);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var funcionario = await _repository.GetByIdAsync(id);
        if (funcionario == null)
            return false;

        _repository.Delete(funcionario);
        await _repository.SaveChangesAsync();
        return true;
    }
}

}
