using Microsoft.EntityFrameworkCore;
using Dominio.Entidades;

namespace Infra.Contexto;

public class LivrosContexto : DbContext
{
    public LivrosContexto(DbContextOptions<LivrosContexto> opcoes) : base(opcoes)
    {
    }

    public DbSet<Livro> Livros { get; set; }
    public DbSet<Autor> Autores { get; set; }
    public DbSet<Genero> Generos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LivrosContexto).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}