using ApiEmpresas.DTOs.Empresa;
using ApiEmpresas.Models;

namespace ApiEmpresas.Services.Interfaces
{
    public interface IEmpresaService
    {
        Task<IEnumerable<EmpresaResponseDTO>> GetAllAsync();
        Task<EmpresaResponseDTO?> GetByIdAsync(Guid id);
        Task<EmpresaResponseDTO> CreateAsync(CreateEmpresaDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<EmpresaResponseDTO?> PatchAsync(Guid id, PatchEmpresaDTO dto);
        Task<EmpresaResponseDTO> AddSetoresAsync(Guid id, AddSetorDTO dto);
        Task<EmpresaResponseDTO> RemoveSetorAsync(Guid id, Guid setorId);
    }
}
