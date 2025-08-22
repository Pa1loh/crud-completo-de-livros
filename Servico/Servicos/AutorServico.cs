using Dominio.Entidades;
using Infra.Contexto;
using Microsoft.EntityFrameworkCore;
using Servico.Dtos;
using Servico.Interfaces;

namespace Servico.Servicos;

public class AutorServico : IAutorServico
{
    private readonly LivrosContexto _contexto;

    public AutorServico(LivrosContexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<AutorDto> CriarAsync(CriarAutorDto dto)
    {
        if (await ExisteNomeAsync(dto.Nome))
            throw new InvalidOperationException("Já existe um autor com este nome");

        var autor = new Autor(Guid.NewGuid(), dto.Nome);

        _contexto.Autores.Add(autor);
        await _contexto.SaveChangesAsync();

        return ConverterParaDto(autor);
    }

    public async Task<AutorDto?> ObterPorIdAsync(Guid id)
    {
        var autor = await _contexto.Autores
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);

        return autor != null ? ConverterParaDto(autor) : null;
    }

    public async Task<IEnumerable<AutorDto>> ObterTodosAsync()
    {
        var autores = await _contexto.Autores
            .AsNoTracking()
            .OrderBy(a => a.Nome)
            .ToListAsync();

        return autores.Select(ConverterParaDto);
    }

    public async Task<AutorDto> AtualizarAsync(Guid id, AtualizarAutorDto dto)
    {
        var autor = await _contexto.Autores
            .FirstOrDefaultAsync(a => a.Id == id);

        if (autor == null)
            throw new InvalidOperationException("Autor não encontrado");

        if (await ExisteNomeAsync(dto.Nome, id))
            throw new InvalidOperationException("Já existe um autor com este nome");

        autor.AlterarNome(dto.Nome);
        await _contexto.SaveChangesAsync();

        return ConverterParaDto(autor);
    }

    public async Task RemoverAsync(Guid id)
    {
        var autor = await _contexto.Autores
            .FirstOrDefaultAsync(a => a.Id == id);

        if (autor == null)
            throw new InvalidOperationException("Autor não encontrado");

        var possuiLivros = await _contexto.Livros
            .AsNoTracking()
            .AnyAsync(l => l.AutorId == id);

        if (possuiLivros)
            throw new InvalidOperationException("Não é possível remover um autor que possui livros associados");

        _contexto.Autores.Remove(autor);
        await _contexto.SaveChangesAsync();
    }

    public async Task<bool> ExisteAsync(Guid id)
    {
        return await _contexto.Autores
            .AsNoTracking()
            .AnyAsync(a => a.Id == id);
    }

    private async Task<bool> ExisteNomeAsync(string nome, Guid? idExcluir = null)
    {
        var query = _contexto.Autores.AsNoTracking().Where(a => a.Nome == nome);

        if (idExcluir.HasValue)
            query = query.Where(a => a.Id != idExcluir.Value);

        return await query.AnyAsync();
    }

    private static AutorDto ConverterParaDto(Autor autor)
    {
        return new AutorDto
        {
            Id = autor.Id,
            Nome = autor.Nome,
            DataCriacao = autor.DataCriacao
        };
    }
}