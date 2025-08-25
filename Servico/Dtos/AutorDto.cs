using System.ComponentModel.DataAnnotations;

namespace Servico.Dtos;

public abstract class AutorBaseDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;
}

public class CriarAutorDto : AutorBaseDto
{
}

public class AtualizarAutorDto : AutorBaseDto
{
}

public class AutorDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}