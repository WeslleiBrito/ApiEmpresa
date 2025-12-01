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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool removed = await _service.DeleteAsync(id);

            return removed ? NoContent() : NotFound();
        }
    }
}
