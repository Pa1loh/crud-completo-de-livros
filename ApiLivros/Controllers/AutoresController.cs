using Microsoft.AspNetCore.Mvc;
using Servico.Dtos;
using Servico.Interfaces;

namespace ApiLivros.Controllers;

[ApiController]
[Route("api/v1/autores")]
public class AutoresControlador : ControllerBase
{
    private readonly IAutorServico _autorServico;

    public AutoresControlador(IAutorServico autorServico)
    {
        _autorServico = autorServico;
    }

    [HttpPost]
    public async Task<ActionResult<AutorDto>> CriarAsync([FromBody] CriarAutorDto dto)
    {
        var autorDto = await _autorServico.CriarAsync(dto);
        return Created($"api/v1/autores/{autorDto.Id}", autorDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AutorDto>> ObterPorIdAsync(Guid id)
    {
        var autorDto = await _autorServico.ObterPorIdAsync(id);
        
        if (autorDto == null)
            return NotFound();

        return Ok(autorDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AutorDto>>> ObterTodosAsync()
    {
        var autoresDto = await _autorServico.ObterTodosAsync();
        return Ok(autoresDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AutorDto>> AtualizarAsync(Guid id, [FromBody] AtualizarAutorDto dto)
    {
        var autorDto = await _autorServico.AtualizarAsync(id, dto);
        return Ok(autorDto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> RemoverAsync(Guid id)
    {
        if (!await _autorServico.ExisteAsync(id))
            return NotFound();

        await _autorServico.RemoverAsync(id);
        return NoContent();
    }
}