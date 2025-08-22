using Microsoft.EntityFrameworkCore;

namespace Infra.Contexto;

public class LivrosContexto : DbContext
{
    public LivrosContexto(DbContextOptions<LivrosContexto> opcoes) : base(opcoes)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações das entidades serão adicionadas aqui
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Fallback caso não seja configurado via DI
            optionsBuilder.UseNpgsql("Host=localhost;Database=livros;Username=postgres;Password=postgres");
        }
    }
}