using System.ComponentModel.DataAnnotations;

namespace Servico.Dtos;

public class CriarLivroDto
{
    [Required(ErrorMessage = "Título é obrigatório")]
    [StringLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
    public string Titulo { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "AutorId é obrigatório")]
    public Guid AutorId { get; set; }
    
    [Required(ErrorMessage = "GeneroId é obrigatório")]
    public Guid GeneroId { get; set; }
}

public class AtualizarLivroDto
{
    [Required(ErrorMessage = "Título é obrigatório")]
    [StringLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
    public string Titulo { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "AutorId é obrigatório")]
    public Guid AutorId { get; set; }
    
    [Required(ErrorMessage = "GeneroId é obrigatório")]
    public Guid GeneroId { get; set; }
}

public class LivroDto
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
