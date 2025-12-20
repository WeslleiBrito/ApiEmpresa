using ApiEmpresas.DTOs.Habilidade;
using ApiEmpresas.Exceptions;
using ApiEmpresas.Models;
using ApiEmpresas.Repositories.Interfaces;
using ApiEmpresas.Services.Interfaces;
using AutoMapper;

public class HabilidadeService : IHabilidadeService
{
    private readonly IHabilidadeRepository _repo;
    private readonly IMapper _mapper;

    public HabilidadeService(
        IHabilidadeRepository repo,
        IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<HabilidadeResponseDTO>> GetAllAsync()
    {
        var habilidades = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<HabilidadeResponseDTO>>(habilidades);
    }

    public async Task<HabilidadeResponseDTO?> GetByIdAsync(Guid id)
    {
        var habilidade = await _repo.GetByIdAsync(id);
        return habilidade == null
            ? null
            : _mapper.Map<HabilidadeResponseDTO>(habilidade);
    }

    public async Task<HabilidadeResponseDTO> CreateAsync(CreateHabilidadeDTO dto)
    {
        if (await _repo.HabilidadeExistsAsync(dto.Nome))
        {
            throw new ValidationException("nome", "Habilidade com esse nome já existe.");
        }

        var habilidade = _mapper.Map<Habilidade>(dto);

        await _repo.AddAsync(habilidade);
        await _repo.SaveChangesAsync();

        return _mapper.Map<HabilidadeResponseDTO>(habilidade);
    }

    public async Task<HabilidadeResponseDTO?> UpdateAsync(Guid id, UpdateHabilidadeDTO dto)
    {
        var habilidade = await _repo.GetByIdAsync(id) ??  throw new NotFoundException("Habilidade não encontrada.");

        _mapper.Map(dto, habilidade);

        await _repo.SaveChangesAsync();

        return _mapper.Map<HabilidadeResponseDTO>(habilidade);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var habilidade = await _repo.GetByIdAsync(id);
        if (habilidade == null)
            return false;

        _repo.Remove(habilidade);
        await _repo.SaveChangesAsync();

        return true;
    }
}
