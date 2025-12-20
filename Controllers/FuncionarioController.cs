using ApiEmpresas.DTOs.Funcionario;
using ApiEmpresas.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiEmpresas.Controllers
{
    [ApiController]
    [Route("api/funcionario")]
    public class FuncionarioController : ControllerBase
    {
        private readonly IFuncionarioService _service;

        public FuncionarioController(IFuncionarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var funcionario = await _service.GetByIdAsync(id);
            return funcionario != null ? Ok(funcionario) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFuncionarioDTO dto)
        {
            var funcionario = await _service.CreateAsync(dto);
            return Ok(funcionario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateFuncionarioDTO dto)
        {
            var funcionario = await _service.UpdateAsync(id, dto);
            return Ok(funcionario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _service.DeleteAsync(id) ? NoContent() : NotFound();
        }

        [HttpPost("{funcionarioId:guid}/setores")]
        public async Task<IActionResult> AddSetores(Guid funcionarioId, [FromBody] AddSetorDTO dto)
        {
            var result = await _service.AddSetoresAsync(funcionarioId, dto);
            return Ok(result);
        }

        [HttpDelete("{funcionarioId:guid}/setores/{setorId:guid}")]
        public async Task<IActionResult> RemoveSetor(Guid funcionarioId, Guid setorId)
        {
            var result = await _service.RemoveSetorAsync(funcionarioId, setorId);
            return Ok(result);
        }

        [HttpPost("{funcionarioId:guid}/habilidades")]
        public async Task<IActionResult> AddHabilidades(Guid funcionarioId, [FromBody] AddHabilidadesFuncionarioDTO dto)
        {
            var result = await _service.AddHabilidadesAsync(funcionarioId, dto);
            return Ok(result);
        }

        [HttpDelete("{funcionarioId:guid}/habilidades/{habilidadeId:guid}")]
        public async Task<IActionResult> RemoveHabilidade(Guid funcionarioId, RemoveHabilidadesFuncionarioDTO dto)
        {
            var result = await _service.RemoveHabilidadesAsync(funcionarioId, dto);
            return Ok(result);
        }
    }
}