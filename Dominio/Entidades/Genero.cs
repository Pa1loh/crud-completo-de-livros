namespace Dominio.Entidades;

public class Genero
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public DateTime DataCriacao { get; private set; }

    public Genero(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
        DataCriacao = DateTime.UtcNow;
    }
    
    private Genero() { }

    public void AlterarNome(string novoNome)
    {
        Nome = novoNome;
    }
}