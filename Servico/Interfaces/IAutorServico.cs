using Servico.Dtos;

namespace Servico.Interfaces;

public interface IAutorServico
{
    Task<AutorDto> CriarAsync(CriarAutorDto dto);
    Task<AutorDto?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<AutorDto>> ObterTodosAsync();
    Task<AutorDto> AtualizarAsync(Guid id, AtualizarAutorDto dto);
    Task RemoverAsync(Guid id);
    Task<bool> ExisteAsync(Guid id);
}
