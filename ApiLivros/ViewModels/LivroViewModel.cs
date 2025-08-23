namespace ApiLivros.ViewModels;

public class LivroViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public Guid AutorId { get; set; }
    public string NomeAutor { get; set; } = string.Empty;
    public Guid GeneroId { get; set; }
    public string NomeGenero { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}