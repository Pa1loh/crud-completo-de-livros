namespace Dominio.Entidades;

public class Genero
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public DateTime DataCriacao { get; private set; }

    public Genero(Guid id, string nome)
    {
        ValidarNome(nome);
        Id = id;
        Nome = nome;
        DataCriacao = DateTime.UtcNow;
    }
    
    private Genero() { }

    public void AlterarNome(string novoNome)
    {
        ValidarNome(novoNome);
        Nome = novoNome;
    }

    private void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do gênero é obrigatório");
        
        if (nome.Length > 50)
            throw new ArgumentException("Nome do gênero deve ter no máximo 50 caracteres");
    }
}