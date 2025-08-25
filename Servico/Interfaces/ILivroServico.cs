using Servico.Dtos;

namespace Servico.Interfaces;

public interface ILivroServico
{
    Task<LivroDto> CriarAsync(CriarLivroDto dto);
    Task<LivroDto?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<LivroDto>> ObterTodosAsync();
    Task<LivroDto> AtualizarAsync(Guid id, AtualizarLivroDto dto);
    Task RemoverAsync(Guid id);
    Task<bool> ExisteAsync(Guid id);
    Task<IEnumerable<LivroDto>> ObterPorAutorAsync(Guid autorId);
    Task<IEnumerable<LivroDto>> ObterPorGeneroAsync(Guid generoId);
}