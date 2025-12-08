using ApiEmpresas.DTOs.Profissao;
using ApiEmpresas.Exceptions;
using ApiEmpresas.Models;
using ApiEmpresas.Repositories.Interfaces;
using ApiEmpresas.Services.Interfaces;
using AutoMapper;


namespace ApiEmpresas.Services.Implementations
{
    public class ProfissaoService : IProfissaoService
{
    private readonly IProfissaoRepository _repository;
    private readonly IMapper _mapper;

    public ProfissaoService(IProfissaoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProfissaoResponseDTO>> GetAllAsync()
    {
        var profissoes = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProfissaoResponseDTO>>(profissoes);
    }

    public async Task<ProfissaoResponseDTO?> GetByIdAsync(Guid id)
    {
        var profissao = await _repository.GetByIdAsync(id);
        return profissao == null ? null : _mapper.Map<ProfissaoResponseDTO>(profissao);
    }

    public async Task<ProfissaoResponseDTO> CreateAsync(CreateProfissaoDTO dto)
    {
        var profissao = _mapper.Map<Profissao>(dto);

        if (await _repository.ExistsByNomeAsync(profissao.Nome!)) throw new ConflictException("Já existe uma profissão com esse nome.");
                
        await _repository.AddAsync(profissao);
        await _repository.SaveChangesAsync();

        return _mapper.Map<ProfissaoResponseDTO>(profissao);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var profissao = await _repository.GetByIdAsync(id);

        if (profissao == null)
            return false;

        _repository.Delete(profissao);
        await _repository.SaveChangesAsync();

        return true;
    }
}

}
