using System.Runtime.CompilerServices;
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
        Task<FuncionarioResponseDTO> AddSetoresAsync(Guid funcionarioId, AddSetorDTO dto);
        Task<FuncionarioResponseDTO> RemoveSetorAsync(Guid funcionarioId, Guid setorId);
        Task<FuncionarioResponseDTO> AddHabilidadesAsync(Guid funcionarioId, AddHabilidadesFuncionarioDTO dto);
        Task<FuncionarioResponseDTO> RemoveHabilidadesAsync(Guid funcionarioId, RemoveHabilidadesFuncionarioDTO dto); 
    }
}
