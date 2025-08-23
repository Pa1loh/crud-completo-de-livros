using Microsoft.AspNetCore.Mvc;
using Servico.Dtos;
using Servico.Interfaces;
using ApiLivros.ViewModels;
using ApiLivros.Extensions;

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
    public async Task<ActionResult<LivroViewModel>> CriarAsync([FromBody] CriarLivroDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var livroDto = await _livroServico.CriarAsync(dto);
        var viewModel = livroDto.ParaViewModel();
        
        return CreatedAtAction(
            nameof(ObterPorIdAsync),
            new { id = viewModel.Id },
            viewModel);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LivroViewModel>> ObterPorIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { mensagem = "ID inválido" });

        var livroDto = await _livroServico.ObterPorIdAsync(id);
        
        if (livroDto == null)
            return NotFound(new { mensagem = "Livro não encontrado" });

        return Ok(livroDto.ParaViewModel());
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LivroViewModel>>> ObterTodosAsync()
    {
        var livrosDto = await _livroServico.ObterTodosAsync();
        var viewModels = livrosDto.Select(dto => dto.ParaViewModel());
        return Ok(viewModels);
    }

    [HttpGet("autor/{autorId:guid}")]
    public async Task<ActionResult<IEnumerable<LivroViewModel>>> ObterPorAutorAsync(Guid autorId)
    {
        if (autorId == Guid.Empty)
            return BadRequest(new { mensagem = "ID do autor inválido" });

        var livrosDto = await _livroServico.ObterPorAutorAsync(autorId);
        var viewModels = livrosDto.Select(dto => dto.ParaViewModel());
        return Ok(viewModels);
    }

    [HttpGet("genero/{generoId:guid}")]
    public async Task<ActionResult<IEnumerable<LivroViewModel>>> ObterPorGeneroAsync(Guid generoId)
    {
        if (generoId == Guid.Empty)
            return BadRequest(new { mensagem = "ID do gênero inválido" });

        var livrosDto = await _livroServico.ObterPorGeneroAsync(generoId);
        var viewModels = livrosDto.Select(dto => dto.ParaViewModel());
        return Ok(viewModels);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<LivroViewModel>> AtualizarAsync(Guid id, [FromBody] AtualizarLivroDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id == Guid.Empty)
            return BadRequest(new { mensagem = "ID inválido" });

        var livroDto = await _livroServico.AtualizarAsync(id, dto);
        return Ok(livroDto.ParaViewModel());
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> RemoverAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { mensagem = "ID inválido" });

        if (!await _livroServico.ExisteAsync(id))
            return NotFound(new { mensagem = "Livro não encontrado" });

        await _livroServico.RemoverAsync(id);
        return NoContent();
    }
}