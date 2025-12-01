using ApiEmpresas.DTOs.Profissao;

namespace ApiEmpresas.Services.Interfaces
{
    public interface IProfissaoService
    {
        Task<IEnumerable<ProfissaoResponseDTO>> GetAllAsync();
        Task<ProfissaoResponseDTO?> GetByIdAsync(Guid id);
        Task<ProfissaoResponseDTO> CreateAsync(CreateProfissaoDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
