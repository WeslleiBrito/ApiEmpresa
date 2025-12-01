using ApiEmpresas.DTOs.Funcionario;

namespace ApiEmpresas.Services.Interfaces
{
    public interface IFuncionarioService
    {
        Task<IEnumerable<FuncionarioResponseDTO>> GetAllAsync();
        Task<FuncionarioResponseDTO?> GetByIdAsync(Guid id);
        Task<FuncionarioResponseDTO> CreateAsync(CreateFuncionarioDTO dto);
        Task<FuncionarioResponseDTO> UpdateAsync(Guid id, UpdateFuncionarioDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
