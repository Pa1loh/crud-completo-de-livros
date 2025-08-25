namespace Dominio.Excecoes;

public sealed class RecursoNaoEncontradoException : ExcecaoDominio
{
    public RecursoNaoEncontradoException(string recurso) 
        : base($"{recurso} não encontrado")
    {
    }
}