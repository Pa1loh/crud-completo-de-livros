using Microsoft.EntityFrameworkCore;
using Infra.Contexto;
using Servico.Interfaces;
using Servico.Servicos;

namespace ApiLivros.Configuracao;

public static class DependenciasDaAplicacao
{
    public static IServiceCollection AdicionarDependencias(
        this IServiceCollection servicos,
        IConfiguration configuracao)
    {
        AdicionarInfraestrutura(servicos, configuracao);
        AdicionarServicos(servicos);

        return servicos;
    }

    private static void AdicionarInfraestrutura(
        IServiceCollection servicos,
        IConfiguration configuracao)
    {
        var stringConexao = configuracao.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("String de conexão 'DefaultConnection' não encontrada");

        servicos.AddDbContext<LivrosContexto>(opcoes =>
            opcoes.UseNpgsql(stringConexao, builder =>
                builder.MigrationsAssembly(typeof(LivrosContexto).Assembly.FullName)));
    }

    private static void AdicionarServicos(IServiceCollection servicos)
    {
        servicos.AddScoped<IGeneroServico, GeneroServico>();
        servicos.AddScoped<IAutorServico, AutorServico>();
        servicos.AddScoped<ILivroServico, LivroServico>();
    }
}