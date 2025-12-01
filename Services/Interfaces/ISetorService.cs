using ApiEmpresas.DTOs.Setor;

namespace ApiEmpresas.Services.Interfaces
{
    public interface ISetorService
    {
        Task<IEnumerable<SetorResponseDTO>> GetAllAsync();
        Task<SetorResponseDTO?> GetByIdAsync(Guid id);
        Task<SetorResponseDTO> CreateAsync(CreateSetorDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
