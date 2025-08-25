namespace Dominio.Excecoes;

public sealed class RecursoDuplicadoException : ExcecaoDominio
{
    public RecursoDuplicadoException(string recurso, string valor) 
        : base($"Já existe {recurso} com {valor}")
    {
    }
}