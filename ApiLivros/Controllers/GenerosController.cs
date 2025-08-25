using Microsoft.AspNetCore.Mvc;
using Servico.Dtos;
using Servico.Interfaces;

namespace ApiLivros.Controllers;

[ApiController]
[Route("api/v1/generos")]
public class GenerosControlador : ControllerBase
{
    private readonly IGeneroServico _generoServico;

    public GenerosControlador(IGeneroServico generoServico)
    {
        _generoServico = generoServico;
    }

    [HttpPost]
    public async Task<ActionResult<GeneroDto>> CriarAsync([FromBody] CriarGeneroDto dto)
    {
        var generoDto = await _generoServico.CriarAsync(dto);
        return Created($"api/v1/generos/{generoDto.Id}", generoDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GeneroDto>> ObterPorIdAsync(Guid id)
    {
        var generoDto = await _generoServico.ObterPorIdAsync(id);
        
        if (generoDto == null)
            return NotFound();

        return Ok(generoDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GeneroDto>>> ObterTodosAsync()
    {
        var generosDto = await _generoServico.ObterTodosAsync();
        return Ok(generosDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<GeneroDto>> AtualizarAsync(Guid id, [FromBody] AtualizarGeneroDto dto)
    {
        var generoDto = await _generoServico.AtualizarAsync(id, dto);
        return Ok(generoDto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> RemoverAsync(Guid id)
    {
        if (!await _generoServico.ExisteAsync(id))
            return NotFound();

        await _generoServico.RemoverAsync(id);
        return NoContent();
    }
}