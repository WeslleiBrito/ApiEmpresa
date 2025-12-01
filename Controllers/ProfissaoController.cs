using ApiEmpresas.DTOs.Profissao;
using ApiEmpresas.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiEmpresas.Controllers
{
    [ApiController]
    [Route("api/profissao")]
    public class ProfissaoController : ControllerBase
    {
        private readonly IProfissaoService _service;

        public ProfissaoController(IProfissaoService service)
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
            var result = await _service.GetByIdAsync(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProfissaoDTO dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _service.DeleteAsync(id) ? NoContent() : NotFound();
        }
    }
}
