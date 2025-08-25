namespace Dominio.Excecoes;

public sealed class RegraDeNegocioException : ExcecaoDominio
{
    public RegraDeNegocioException(string mensagem) : base(mensagem)
    {
    }
}