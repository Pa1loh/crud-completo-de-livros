namespace Dominio.Entidades;

public class Autor
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public DateTime DataCriacao { get; private set; }

    public Autor(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
        DataCriacao = DateTime.UtcNow;
    }

    private Autor() { }

    public void AlterarNome(string novoNome)
    {
        Nome = novoNome;
    }
}