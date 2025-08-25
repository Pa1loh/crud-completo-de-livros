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
        Id = id;
        Titulo = titulo;
        AutorId = autorId;
        GeneroId = generoId;
        DataCriacao = DateTime.UtcNow;
    }

    private Livro() { }

    public void Atualizar(string novoTitulo, Guid novoAutorId, Guid novoGeneroId)
    {
        Titulo = novoTitulo;
        AutorId = novoAutorId;
        GeneroId = novoGeneroId;
        DataAtualizacao = DateTime.UtcNow;
    }
}