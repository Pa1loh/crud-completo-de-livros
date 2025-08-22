using Microsoft.Extensions.DependencyInjection;
using Servico.Interfaces;
using Servico.Servicos;

namespace Servico.Configuracao;

public static class ServicoDependencias
{
    public static IServiceCollection AdicionarServicos(this IServiceCollection services)
    {
        services.AddScoped<IGeneroServico, GeneroServico>();
        services.AddScoped<IAutorServico, AutorServico>();
        services.AddScoped<ILivroServico, LivroServico>();

        return services;
    }
}
