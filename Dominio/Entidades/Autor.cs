namespace Dominio.Entidades;

public class Autor
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public DateTime DataCriacao { get; private set; }

    public Autor(Guid id, string nome)
    {
        ValidarNome(nome);
        Id = id;
        Nome = nome;
        DataCriacao = DateTime.UtcNow;
    }

    private Autor() { }

    public void AlterarNome(string novoNome)
    {
        ValidarNome(novoNome);
        Nome = novoNome;
    }

    private void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do autor é obrigatório");
        
        if (nome.Length > 100)
            throw new ArgumentException("Nome do autor deve ter no máximo 100 caracteres");
    }
}