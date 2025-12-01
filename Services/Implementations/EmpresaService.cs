using ApiEmpresas.DTOs.Empresa;
using ApiEmpresas.Models;
using ApiEmpresas.Repositories.Interfaces;
using ApiEmpresas.Services.Interfaces;
using AutoMapper;

namespace ApiEmpresas.Services.Implementations
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepository _repo;
        private readonly ISetorRepository _setorRepo;
        private readonly IMapper _mapper;

        public EmpresaService(
            IEmpresaRepository repo,
            ISetorRepository setorRepo,
            IMapper mapper)
        {
            _repo = repo;
            _setorRepo = setorRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmpresaResponseDTO>> GetAllAsync()
        {
            var empresas = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<EmpresaResponseDTO>>(empresas);
        }

        public async Task<EmpresaResponseDTO?> GetByIdAsync(Guid id)
        {
            var empresa = await _repo.GetEmpresaCompletaAsync(id);
            if (empresa == null)
                return null;

            return _mapper.Map<EmpresaResponseDTO>(empresa);
        }

        public async Task<EmpresaResponseDTO> CreateAsync(CreateEmpresaDTO dto)
        {
            // mapear
            var empresa = _mapper.Map<Empresa>(dto);

            // criar vínculos empresa-setor
            empresa.Setores = dto.SetoresIds
                .Select(setorId => new EmpresaSetor
                {
                    SetorId = setorId
                }).ToList();

            await _repo.AddAsync(empresa);
            await _repo.SaveChangesAsync();

            // recarregar dados já com Setor incluso
            var empresaCompleta = await _repo.GetEmpresaCompletaAsync(empresa.Id);

            return _mapper.Map<EmpresaResponseDTO>(empresaCompleta);
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var empresa = await _repo.GetByIdAsync(id);
            if (empresa == null)
                return false;

            _repo.Delete(empresa);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
