using Dominio.Entidades;
using Infra.Contexto;
using Microsoft.EntityFrameworkCore;
using Servico.Dtos;
using Servico.Interfaces;

namespace Servico.Servicos;

public class LivroServico : ILivroServico
{
    private readonly LivrosContexto _contexto;

    public LivroServico(LivrosContexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<LivroDto> CriarAsync(CriarLivroDto dto)
    {
        if (await ExisteTituloAsync(dto.Titulo))
            throw new InvalidOperationException("Já existe um livro com este título");

        await ValidarReferencasAsync(dto.AutorId, dto.GeneroId);

        var livro = new Livro(Guid.NewGuid(), dto.Titulo, dto.AutorId, dto.GeneroId);

        _contexto.Livros.Add(livro);
        await _contexto.SaveChangesAsync();

        return await ObterLivroComReferencasAsync(livro.Id);
    }

    public async Task<LivroDto?> ObterPorIdAsync(Guid id)
    {
        var livro = await _contexto.Livros
            .AsNoTracking()
            .Include(l => l.Autor)
            .Include(l => l.Genero)
            .FirstOrDefaultAsync(l => l.Id == id);

        return livro != null ? ConverterParaDto(livro) : null;
    }

    public async Task<IEnumerable<LivroDto>> ObterTodosAsync()
    {
        var livros = await _contexto.Livros
            .AsNoTracking()
            .Include(l => l.Autor)
            .Include(l => l.Genero)
            .OrderBy(l => l.Titulo)
            .ToListAsync();

        return livros.Select(ConverterParaDto);
    }

    public async Task<LivroDto> AtualizarAsync(Guid id, AtualizarLivroDto dto)
    {
        var livro = await _contexto.Livros
            .FirstOrDefaultAsync(l => l.Id == id);

        if (livro == null)
            throw new InvalidOperationException("Livro não encontrado");

        await ValidarReferencasAsync(dto.AutorId, dto.GeneroId);

        livro.Atualizar(dto.Titulo, dto.AutorId, dto.GeneroId);
        await _contexto.SaveChangesAsync();

        return await ObterLivroComReferencasAsync(livro.Id);
    }

    public async Task RemoverAsync(Guid id)
    {
        var livro = await _contexto.Livros
            .FirstOrDefaultAsync(l => l.Id == id);

        if (livro == null)
            throw new InvalidOperationException("Livro não encontrado");

        _contexto.Livros.Remove(livro);
        await _contexto.SaveChangesAsync();
    }

    public async Task<bool> ExisteAsync(Guid id)
    {
        return await _contexto.Livros
            .AsNoTracking()
            .AnyAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<LivroDto>> ObterPorAutorAsync(Guid autorId)
    {
        var livros = await _contexto.Livros
            .AsNoTracking()
            .Include(l => l.Autor)
            .Include(l => l.Genero)
            .Where(l => l.AutorId == autorId)
            .OrderBy(l => l.Titulo)
            .ToListAsync();

        return livros.Select(ConverterParaDto);
    }

    public async Task<IEnumerable<LivroDto>> ObterPorGeneroAsync(Guid generoId)
    {
        var livros = await _contexto.Livros
            .AsNoTracking()
            .Include(l => l.Autor)
            .Include(l => l.Genero)
            .Where(l => l.GeneroId == generoId)
            .OrderBy(l => l.Titulo)
            .ToListAsync();

        return livros.Select(ConverterParaDto);
    }

    private async Task ValidarReferencasAsync(Guid autorId, Guid generoId)
    {
        var autorExiste = await _contexto.Autores
            .AsNoTracking()
            .AnyAsync(a => a.Id == autorId);

        if (!autorExiste)
            throw new InvalidOperationException("Autor não encontrado");

        var generoExiste = await _contexto.Generos
            .AsNoTracking()
            .AnyAsync(g => g.Id == generoId);

        if (!generoExiste)
            throw new InvalidOperationException("Gênero não encontrado");
    }

    private async Task<LivroDto> ObterLivroComReferencasAsync(Guid id)
    {
        var livro = await _contexto.Livros
            .AsNoTracking()
            .Include(l => l.Autor)
            .Include(l => l.Genero)
            .FirstAsync(l => l.Id == id);

        return ConverterParaDto(livro);
    }

    private static LivroDto ConverterParaDto(Livro livro)
    {
        return new LivroDto
        {
            Id = livro.Id,
            Titulo = livro.Titulo,
            AutorId = livro.AutorId,
            NomeAutor = livro.Autor?.Nome ?? string.Empty,
            GeneroId = livro.GeneroId,
            NomeGenero = livro.Genero?.Nome ?? string.Empty,
            DataCriacao = livro.DataCriacao,
            DataAtualizacao = livro.DataAtualizacao
        };
    }

    private async Task<bool> ExisteTituloAsync(string titulo)
    {
        return await _contexto.Livros
            .AsNoTracking()
            .AnyAsync(l => l.Titulo == titulo);
    }
}