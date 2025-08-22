using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infra.Contexto;

namespace Infra.Configuracao;

public static class InfraestruturaDependencias
{
    public static IServiceCollection AdicionarInfraestrutura(
        this IServiceCollection servicos, 
        IConfiguration configuracao)
    {
        var stringConexao = configuracao.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("String de conexão 'DefaultConnection' não encontrada");

        servicos.AddDbContext<LivrosContexto>(opcoes =>
            opcoes.UseNpgsql(stringConexao, builder =>
                builder.MigrationsAssembly(typeof(LivrosContexto).Assembly.FullName)));

        return servicos;
    }
}