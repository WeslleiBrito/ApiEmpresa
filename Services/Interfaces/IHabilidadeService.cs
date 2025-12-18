using ApiEmpresas.DTOs.Habilidade;

namespace ApiEmpresas.Services.Interfaces
{
    public interface IHabilidadeService
    {
        Task<IEnumerable<HabilidadeResponseDTO>> GetAllAsync();
        Task<HabilidadeResponseDTO?> GetByIdAsync(Guid id);
        Task<HabilidadeResponseDTO> CreateAsync(CreateHabilidadeDTO dto);
        Task<HabilidadeResponseDTO?> UpdateAsync(Guid id, UpdateHabilidadeDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
