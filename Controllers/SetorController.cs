using ApiEmpresas.DTOs.Setor;
using ApiEmpresas.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiEmpresas.Controllers
{
    [ApiController]
    [Route("api/setor")]
    public class SetorController : ControllerBase
    {
        private readonly ISetorService _service;

        public SetorController(ISetorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var setor = await _service.GetByIdAsync(id);
            if (setor == null)
                return NotFound();

            return Ok(setor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSetorDTO dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSetorDTO dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var apagou = await _service.DeleteAsync(id);
            if (!apagou)
                return BadRequest(new { error = "Não é possível excluir o setor. Existem vínculos ativos." });

            return NoContent();
        }
    }
}
