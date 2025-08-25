using Servico.Dtos;

namespace Servico.Interfaces;

public interface IGeneroServico
{
    Task<GeneroDto> CriarAsync(CriarGeneroDto dto);
    Task<GeneroDto?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<GeneroDto>> ObterTodosAsync();
    Task<GeneroDto> AtualizarAsync(Guid id, AtualizarGeneroDto dto);
    Task RemoverAsync(Guid id);
    Task<bool> ExisteAsync(Guid id);
}