using Microsoft.AspNetCore.Mvc;
using Servico.Dtos;
using Servico.Interfaces;
using ApiLivros.ViewModels;
using ApiLivros.Extensions;

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
    public async Task<ActionResult<AutorViewModel>> CriarAsync([FromBody] CriarAutorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var autorDto = await _autorServico.CriarAsync(dto);
        var viewModel = autorDto.ParaViewModel();
        
        return CreatedAtAction(
            nameof(ObterPorIdAsync),
            new { id = viewModel.Id },
            viewModel);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AutorViewModel>> ObterPorIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { mensagem = "ID inválido" });

        var autorDto = await _autorServico.ObterPorIdAsync(id);
        
        if (autorDto == null)
            return NotFound(new { mensagem = "Autor não encontrado" });

        return Ok(autorDto.ParaViewModel());
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AutorViewModel>>> ObterTodosAsync()
    {
        var autoresDto = await _autorServico.ObterTodosAsync();
        var viewModels = autoresDto.Select(dto => dto.ParaViewModel());
        return Ok(viewModels);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AutorViewModel>> AtualizarAsync(Guid id, [FromBody] AtualizarAutorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id == Guid.Empty)
            return BadRequest(new { mensagem = "ID inválido" });

        var autorDto = await _autorServico.AtualizarAsync(id, dto);
        return Ok(autorDto.ParaViewModel());
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> RemoverAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { mensagem = "ID inválido" });

        if (!await _autorServico.ExisteAsync(id))
            return NotFound(new { mensagem = "Autor não encontrado" });

        await _autorServico.RemoverAsync(id);
        return NoContent();
    }
}