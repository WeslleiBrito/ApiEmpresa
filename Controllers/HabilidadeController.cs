using ApiEmpresas.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ApiEmpresas.DTOs.Habilidade;

namespace ApiEmpresas.Controllers
{
    [ApiController]
    [Route("api/habilidades")]
    public class HabilidadeController : ControllerBase
    {
        private readonly IHabilidadeService _service;

        public HabilidadeController(IHabilidadeService service)
        {
            _service = service;
        }

        // POST: api/habilidades
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHabilidadeDTO dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // GET: api/habilidades
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // GET: api/habilidades/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        // DELETE: api/habilidades/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { message = "Habilidade removida com sucesso." });
        }

        // PUT: api/habilidades/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHabilidadeDTO dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null)
                return NotFound(new { message = "Habilidade não encontrada." });

            return Ok(result);
        }
    }
}
