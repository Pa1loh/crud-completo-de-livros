using Microsoft.EntityFrameworkCore;
using Infra.Contexto;

namespace Teste.Base;

public abstract class TesteComBancoMemoria
{
    protected LivrosContexto CriarContextoEmMemoria()
    {
        var opcoes = new DbContextOptionsBuilder<LivrosContexto>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new LivrosContexto(opcoes);
    }

    protected async Task LimparBancoAsync(LivrosContexto contexto)
    {
        contexto.Livros.RemoveRange(contexto.Livros);
        contexto.Autores.RemoveRange(contexto.Autores);
        contexto.Generos.RemoveRange(contexto.Generos);
        await contexto.SaveChangesAsync();
    }
}