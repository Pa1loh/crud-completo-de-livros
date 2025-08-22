using System.ComponentModel.DataAnnotations;

namespace Servico.Dtos;

public class CriarGeneroDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
    public string Nome { get; set; } = string.Empty;
}

public class AtualizarGeneroDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
    public string Nome { get; set; } = string.Empty;
}

public class GeneroDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}
