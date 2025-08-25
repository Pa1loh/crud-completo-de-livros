using Microsoft.AspNetCore.Mvc;
using Servico.Dtos;
using Servico.Interfaces;

namespace ApiLivros.Controllers;

[ApiController]
[Route("api/v1/livros")]
public class LivrosControlador : ControllerBase
{
    private readonly ILivroServico _livroServico;

    public LivrosControlador(ILivroServico livroServico)
    {
        _livroServico = livroServico;
    }

    [HttpPost]
    public async Task<ActionResult<LivroDto>> CriarAsync([FromBody] CriarLivroDto dto)
    {
        var livroDto = await _livroServico.CriarAsync(dto);
        return Created($"api/v1/livros/{livroDto.Id}", livroDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LivroDto>> ObterPorIdAsync(Guid id)
    {
        var livroDto = await _livroServico.ObterPorIdAsync(id);
        
        if (livroDto == null)
            return NotFound();

        return Ok(livroDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LivroDto>>> ObterTodosAsync()
    {
        var livrosDto = await _livroServico.ObterTodosAsync();
        return Ok(livrosDto);
    }

    [HttpGet("autor/{autorId:guid}")]
    public async Task<ActionResult<IEnumerable<LivroDto>>> ObterPorAutorAsync(Guid autorId)
    {
        var livrosDto = await _livroServico.ObterPorAutorAsync(autorId);
        return Ok(livrosDto);
    }

    [HttpGet("genero/{generoId:guid}")]
    public async Task<ActionResult<IEnumerable<LivroDto>>> ObterPorGeneroAsync(Guid generoId)
    {
        var livrosDto = await _livroServico.ObterPorGeneroAsync(generoId);
        return Ok(livrosDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<LivroDto>> AtualizarAsync(Guid id, [FromBody] AtualizarLivroDto dto)
    {
        var livroDto = await _livroServico.AtualizarAsync(id, dto);
        return Ok(livroDto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> RemoverAsync(Guid id)
    {
        if (!await _livroServico.ExisteAsync(id))
            return NotFound();

        await _livroServico.RemoverAsync(id);
        return NoContent();
    }
}