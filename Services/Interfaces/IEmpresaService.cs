using ApiEmpresas.DTOs.Empresa;


namespace ApiEmpresas.Services.Interfaces
{
    public interface IEmpresaService
    {
        Task<IEnumerable<EmpresaResponseDTO>> GetAllAsync();
        Task<EmpresaResponseDTO?> GetByIdAsync(Guid id);
        Task<EmpresaResponseDTO> CreateAsync(CreateEmpresaDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<EmpresaResponseDTO?> PatchAsync(Guid id, PatchEmpresaDTO dto);
        Task<EmpresaResponseDTO> AddSetoresAsync(Guid empresaId, AddSetorDTO dto);
        Task RemoveSetoresAsync(Guid empresaId, RemoveSetorDTO dto);
        Task<EmpresaResponseDTO> AlocarFuncionarioSetorAsync(Guid empresaId, AddFuncionarioEmpresaDTO dto);
        Task<EmpresaResponseDTO> RemoveFuncionarioAsync(Guid empresaId, RemoveFuncionarioEmpresaDTO dto);
    }
}
