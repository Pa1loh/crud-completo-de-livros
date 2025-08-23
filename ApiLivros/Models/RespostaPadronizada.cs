namespace ApiLivros.Models;

public class RespostaPadronizada<T>
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public T? Dados { get; set; }
    public List<string> Erros { get; set; } = new();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static RespostaPadronizada<T> ComSucesso(T dados, string mensagem = "Operação realizada com sucesso")
    {
        return new RespostaPadronizada<T>
        {
            Sucesso = true,
            Mensagem = mensagem,
            Dados = dados
        };
    }

    public static RespostaPadronizada<T> ComErro(string mensagem, List<string>? erros = null)
    {
        return new RespostaPadronizada<T>
        {
            Sucesso = false,
            Mensagem = mensagem,
            Erros = erros ?? new List<string>()
        };
    }
}