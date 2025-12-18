using ApiEmpresas.DTOs.Empresa;
using ApiEmpresas.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiEmpresas.Controllers
{
    [ApiController]
    [Route("api/empresa")]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaService _service;

        public EmpresaController(IEmpresaService service)
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
            var empresa = await _service.GetByIdAsync(id);

            if (empresa == null)
                return NotFound();

            return Ok(empresa);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmpresaDTO dto)
        {
            var empresa = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = empresa.Id }, empresa);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletou = await _service.DeleteAsync(id);

            if (!deletou)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] PatchEmpresaDTO dto)
        {
            var result = await _service.PatchAsync(id, dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{empresaId:guid}/setores/{setorId:guid}")]
        public async Task<IActionResult> RemoveSetor(Guid empresaId, Guid setorId)
        {
            var empresaAtualizada = await _service.RemoveSetorAsync(empresaId, setorId);
            return Ok(empresaAtualizada);
        }

        [HttpPost("{empresaId:guid}/setores")]
        public async Task<IActionResult> AddSetores(Guid empresaId, [FromBody] AddSetorDTO dto)
        {
            var empresaAtualizada = await _service.AddSetoresAsync(empresaId, dto);
            return Ok(empresaAtualizada);
        }

    }
}
