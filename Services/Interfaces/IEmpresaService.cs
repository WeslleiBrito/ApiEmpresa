using ApiEmpresas.DTOs.Empresa;

namespace ApiEmpresas.Services.Interfaces
{
    public interface IEmpresaService
    {
        Task<IEnumerable<EmpresaResponseDTO>> GetAllAsync();
        Task<EmpresaResponseDTO?> GetByIdAsync(Guid id);
        Task<EmpresaResponseDTO> CreateAsync(CreateEmpresaDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
