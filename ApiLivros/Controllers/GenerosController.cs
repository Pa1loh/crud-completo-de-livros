using Microsoft.AspNetCore.Mvc;
using Servico.Dtos;
using Servico.Interfaces;
using ApiLivros.ViewModels;
using ApiLivros.Extensions;

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
    public async Task<ActionResult<GeneroViewModel>> CriarAsync([FromBody] CriarGeneroDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var generoDto = await _generoServico.CriarAsync(dto);
        var viewModel = generoDto.ParaViewModel();
        
        return CreatedAtAction(
            nameof(ObterPorIdAsync),
            new { id = viewModel.Id },
            viewModel);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GeneroViewModel>> ObterPorIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { mensagem = "ID inválido" });

        var generoDto = await _generoServico.ObterPorIdAsync(id);
        
        if (generoDto == null)
            return NotFound(new { mensagem = "Gênero não encontrado" });

        return Ok(generoDto.ParaViewModel());
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GeneroViewModel>>> ObterTodosAsync()
    {
        var generosDto = await _generoServico.ObterTodosAsync();
        var viewModels = generosDto.Select(dto => dto.ParaViewModel());
        return Ok(viewModels);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<GeneroViewModel>> AtualizarAsync(Guid id, [FromBody] AtualizarGeneroDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id == Guid.Empty)
            return BadRequest(new { mensagem = "ID inválido" });

        var generoDto = await _generoServico.AtualizarAsync(id, dto);
        return Ok(generoDto.ParaViewModel());
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> RemoverAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { mensagem = "ID inválido" });

        if (!await _generoServico.ExisteAsync(id))
            return NotFound(new { mensagem = "Gênero não encontrado" });

        await _generoServico.RemoverAsync(id);
        return NoContent();
    }
}