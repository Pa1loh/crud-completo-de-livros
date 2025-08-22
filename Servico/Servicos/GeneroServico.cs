using Dominio.Entidades;
using Infra.Contexto;
using Microsoft.EntityFrameworkCore;
using Servico.Dtos;
using Servico.Interfaces;

namespace Servico.Servicos;

public class GeneroServico : IGeneroServico
{
    private readonly LivrosContexto _contexto;

    public GeneroServico(LivrosContexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<GeneroDto> CriarAsync(CriarGeneroDto dto)
    {
        if (await ExisteNomeAsync(dto.Nome))
            throw new InvalidOperationException("Já existe um gênero com este nome");

        var genero = new Genero(Guid.NewGuid(), dto.Nome);

        _contexto.Generos.Add(genero);
        await _contexto.SaveChangesAsync();

        return ConverterParaDto(genero);
    }

    public async Task<GeneroDto?> ObterPorIdAsync(Guid id)
    {
        var genero = await _contexto.Generos
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id);

        return genero != null ? ConverterParaDto(genero) : null;
    }

    public async Task<IEnumerable<GeneroDto>> ObterTodosAsync()
    {
        var generos = await _contexto.Generos
            .AsNoTracking()
            .OrderBy(g => g.Nome)
            .ToListAsync();

        return generos.Select(ConverterParaDto);
    }

    public async Task<GeneroDto> AtualizarAsync(Guid id, AtualizarGeneroDto dto)
    {
        var genero = await _contexto.Generos
            .FirstOrDefaultAsync(g => g.Id == id);

        if (genero == null)
            throw new InvalidOperationException("Gênero não encontrado");

        if (await ExisteNomeAsync(dto.Nome, id))
            throw new InvalidOperationException("Já existe um gênero com este nome");

        genero.AlterarNome(dto.Nome);
        await _contexto.SaveChangesAsync();

        return ConverterParaDto(genero);
    }

    public async Task RemoverAsync(Guid id)
    {
        var genero = await _contexto.Generos
            .FirstOrDefaultAsync(g => g.Id == id);

        if (genero == null)
            throw new InvalidOperationException("Gênero não encontrado");

        var possuiLivros = await _contexto.Livros
            .AsNoTracking()
            .AnyAsync(l => l.GeneroId == id);

        if (possuiLivros)
            throw new InvalidOperationException("Não é possível remover um gênero que possui livros associados");

        _contexto.Generos.Remove(genero);
        await _contexto.SaveChangesAsync();
    }

    public async Task<bool> ExisteAsync(Guid id)
    {
        return await _contexto.Generos
            .AsNoTracking()
            .AnyAsync(g => g.Id == id);
    }

    private async Task<bool> ExisteNomeAsync(string nome, Guid? idExcluir = null)
    {
        var query = _contexto.Generos.AsNoTracking().Where(g => g.Nome == nome);

        if (idExcluir.HasValue)
            query = query.Where(g => g.Id != idExcluir.Value);

        return await query.AnyAsync();
    }

    private static GeneroDto ConverterParaDto(Genero genero)
    {
        return new GeneroDto
        {
            Id = genero.Id,
            Nome = genero.Nome,
            DataCriacao = genero.DataCriacao
        };
    }
}