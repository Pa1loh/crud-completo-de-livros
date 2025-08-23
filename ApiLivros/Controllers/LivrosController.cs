using Microsoft.AspNetCore.Mvc;
using Servico.Dtos;
using Servico.Interfaces;

namespace ApiLivros.Controllers;

[ApiController]
[Route("api/livros")]
public class LivrosController : ControllerBase
{
    private readonly ILivroServico _livroServico;

    public LivrosController(ILivroServico livroServico)
    {
        _livroServico = livroServico;
    }

    [HttpPost]
    public async Task<ActionResult<LivroDto>> CriarAsync([FromBody] CriarLivroDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var livro = await _livroServico.CriarAsync(dto);
            return Created($"/api/livros/{livro.Id}", livro);
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
    public async Task<ActionResult<LivroDto>> ObterPorIdAsync(Guid id)
    {
        try
        {
            var livro = await _livroServico.ObterPorIdAsync(id);
            
            if (livro == null)
                return NotFound(new { mensagem = "Livro não encontrado" });

            return Ok(livro);
        }
        catch (Exception)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor" });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LivroDto>>> ObterTodosAsync()
    {
        try
        {
            var livros = await _livroServico.ObterTodosAsync();
            return Ok(livros);
        }
        catch (Exception)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor" });
        }
    }

    [HttpGet("autor/{autorId:guid}")]
    public async Task<ActionResult<IEnumerable<LivroDto>>> ObterPorAutorAsync(Guid autorId)
    {
        try
        {
            var livros = await _livroServico.ObterPorAutorAsync(autorId);
            return Ok(livros);
        }
        catch (Exception)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor" });
        }
    }

    [HttpGet("genero/{generoId:guid}")]
    public async Task<ActionResult<IEnumerable<LivroDto>>> ObterPorGeneroAsync(Guid generoId)
    {
        try
        {
            var livros = await _livroServico.ObterPorGeneroAsync(generoId);
            return Ok(livros);
        }
        catch (Exception)
        {
            return StatusCode(500, new { mensagem = "Erro interno do servidor" });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<LivroDto>> AtualizarAsync(Guid id, [FromBody] AtualizarLivroDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var livro = await _livroServico.AtualizarAsync(id, dto);
            return Ok(livro);
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
            if (!await _livroServico.ExisteAsync(id))
                return NotFound(new { mensagem = "Livro não encontrado" });

            await _livroServico.RemoverAsync(id);
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
