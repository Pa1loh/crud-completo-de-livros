namespace Dominio.Entidades;

public class Livro
{
    public Guid Id { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public Guid AutorId { get; private set; }
    public Guid GeneroId { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataAtualizacao { get; private set; }
    public Autor? Autor { get; private set; }
    public Genero? Genero { get; private set; }

    public Livro(Guid id, string titulo, Guid autorId, Guid generoId)
    {
        ValidarInvariantes(titulo, autorId, generoId);
        Id = id;
        Titulo = titulo;
        AutorId = autorId;
        GeneroId = generoId;
        DataCriacao = DateTime.UtcNow;
    }

    private Livro() { }

    public void AlterarTitulo(string novoTitulo)
    {
        ValidarTitulo(novoTitulo);
        Titulo = novoTitulo;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void AlterarAutor(Guid novoAutorId)
    {
        ValidarAutorId(novoAutorId);
        AutorId = novoAutorId;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void AlterarGenero(Guid novoGeneroId)
    {
        ValidarGeneroId(novoGeneroId);
        GeneroId = novoGeneroId;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void Atualizar(string novoTitulo, Guid novoAutorId, Guid novoGeneroId)
    {
        ValidarInvariantes(novoTitulo, novoAutorId, novoGeneroId);
        Titulo = novoTitulo;
        AutorId = novoAutorId;
        GeneroId = novoGeneroId;
        DataAtualizacao = DateTime.UtcNow;
    }

    private void ValidarInvariantes(string titulo, Guid autorId, Guid generoId)
    {
        ValidarTitulo(titulo);
        ValidarAutorId(autorId);
        ValidarGeneroId(generoId);
    }

    private void ValidarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título é obrigatório");
        
        if (titulo.Length > 200)
            throw new ArgumentException("Título deve ter no máximo 200 caracteres");
    }

    private void ValidarAutorId(Guid autorId)
    {
        if (autorId == Guid.Empty)
            throw new ArgumentException("ID do autor é obrigatório");
    }

    private void ValidarGeneroId(Guid generoId)
    {
        if (generoId == Guid.Empty)
            throw new ArgumentException("ID do gênero é obrigatório");
    }
}