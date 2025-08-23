using Microsoft.AspNetCore.Mvc;
using Servico.Dtos;
using Servico.Interfaces;

namespace ApiLivros.Controllers;

[ApiController]
[Route("api/generos")]
public class GenerosController : ControllerBase
{
    private readonly IGeneroServico _generoServico;

    public GenerosController(IGeneroServico generoServico)
    {
        _generoServico = generoServico;
    }

    [HttpPost]
    public async Task<ActionResult<GeneroDto>> CriarAsync([FromBody] CriarGeneroDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var genero = await _generoServico.CriarAsync(dto);
            return Created($"/api/generos/{genero.Id}", genero);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor" });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GeneroDto>> ObterPorIdAsync(Guid id)
    {
        try
        {
            var genero = await _generoServico.ObterPorIdAsync(id);
            
            if (genero == null)
                return NotFound(new { mensagem = "Gênero não encontrado" });

            return Ok(genero);
        }
        catch (Exception)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor" });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GeneroDto>>> ObterTodosAsync()
    {
        try
        {
            var generos = await _generoServico.ObterTodosAsync();
            return Ok(generos);
        }
        catch (Exception)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor" });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<GeneroDto>> AtualizarAsync(Guid id, [FromBody] AtualizarGeneroDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var genero = await _generoServico.AtualizarAsync(id, dto);
            return Ok(genero);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor" });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> RemoverAsync(Guid id)
    {
        try
        {
            if (!await _generoServico.ExisteAsync(id))
                return NotFound(new { mensagem = "Gênero não encontrado" });

            await _generoServico.RemoverAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor" });
        }
    }
}
