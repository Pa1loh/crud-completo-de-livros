using Microsoft.AspNetCore.Mvc;
using Servico.Dtos;
using Servico.Interfaces;

namespace ApiLivros.Controllers;

[ApiController]
[Route("api/autores")]
public class AutoresController : ControllerBase
{
    private readonly IAutorServico _autorServico;

    public AutoresController(IAutorServico autorServico)
    {
        _autorServico = autorServico;
    }

    [HttpPost]
    public async Task<ActionResult<AutorDto>> CriarAsync([FromBody] CriarAutorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var autor = await _autorServico.CriarAsync(dto);
            return Created($"/api/autores/{autor.Id}", autor);
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
    public async Task<ActionResult<AutorDto>> ObterPorIdAsync(Guid id)
    {
        try
        {
            var autor = await _autorServico.ObterPorIdAsync(id);
            
            if (autor == null)
                return NotFound(new { mensagem = "Autor não encontrado" });

            return Ok(autor);
        }
        catch (Exception)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor" });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AutorDto>>> ObterTodosAsync()
    {
        try
        {
            var autores = await _autorServico.ObterTodosAsync();
            return Ok(autores);
        }
        catch (Exception)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor" });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AutorDto>> AtualizarAsync(Guid id, [FromBody] AtualizarAutorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var autor = await _autorServico.AtualizarAsync(id, dto);
            return Ok(autor);
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
            if (!await _autorServico.ExisteAsync(id))
                return NotFound(new { mensagem = "Autor não encontrado" });

            await _autorServico.RemoverAsync(id);
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
