using System.ComponentModel.DataAnnotations;

namespace Servico.Dtos;

public abstract class GeneroBaseDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
    public string Nome { get; set; } = string.Empty;
}

public class CriarGeneroDto : GeneroBaseDto
{
}

public class AtualizarGeneroDto : GeneroBaseDto
{
}

public class GeneroDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}