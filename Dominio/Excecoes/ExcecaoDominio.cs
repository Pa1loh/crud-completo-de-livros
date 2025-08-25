namespace Dominio.Excecoes;

public abstract class ExcecaoDominio : Exception
{
    protected ExcecaoDominio(string mensagem) : base(mensagem)
    {
    }
}