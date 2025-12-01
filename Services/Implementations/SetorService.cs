using ApiEmpresas.DTOs.Setor;
using ApiEmpresas.Models;
using ApiEmpresas.Repositories.Interfaces;
using ApiEmpresas.Services.Interfaces;
using AutoMapper;

namespace ApiEmpresas.Services.Implementations
{
    public class SetorService : ISetorService
    {
        private readonly ISetorRepository _repo;
        private readonly IMapper _mapper;

        public SetorService(ISetorRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SetorResponseDTO>> GetAllAsync()
        {
            var setores = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<SetorResponseDTO>>(setores);
        }

        public async Task<SetorResponseDTO?> GetByIdAsync(Guid id)
        {
            var setor = await _repo.GetByIdAsync(id);
            if (setor == null) return null;
            return _mapper.Map<SetorResponseDTO>(setor);
        }

        public async Task<SetorResponseDTO> CreateAsync(CreateSetorDTO dto)
        {
            var setor = _mapper.Map<Setor>(dto);
            await _repo.AddAsync(setor);
            await _repo.SaveChangesAsync();
            return _mapper.Map<SetorResponseDTO>(setor);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var setor = await _repo.GetByIdAsync(id);
            if (setor == null) return false;
            _repo.Delete(setor);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
