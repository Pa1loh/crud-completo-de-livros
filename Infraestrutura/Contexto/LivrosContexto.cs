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
        ConfigurarGenero(modelBuilder);
        ConfigurarAutor(modelBuilder);
        ConfigurarLivro(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void ConfigurarGenero(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genero>(builder =>
        {
            builder.ToTable("Generos");
            
            builder.HasKey(g => g.Id);
            
            builder.Property(g => g.Nome)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(g => g.DataCriacao)
                .IsRequired();

            builder.HasIndex(g => g.Nome)
                .IsUnique()
                .HasDatabaseName("IX_Generos_Nome");
        });
    }

    private void ConfigurarAutor(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>(builder =>
        {
            builder.ToTable("Autores");
            
            builder.HasKey(a => a.Id);
            
            builder.Property(a => a.Nome)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.DataCriacao)
                .IsRequired();

            builder.HasIndex(a => a.Nome)
                .HasDatabaseName("IX_Autores_Nome");
        });
    }

    private void ConfigurarLivro(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Livro>(builder =>
        {
            builder.ToTable("Livros");
            
            builder.HasKey(l => l.Id);
            
            builder.Property(l => l.Titulo)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(l => l.AutorId)
                .IsRequired();

            builder.Property(l => l.GeneroId)
                .IsRequired();

            builder.Property(l => l.DataCriacao)
                .IsRequired();

            builder.Property(l => l.DataAtualizacao)
                .IsRequired(false);

            // Configuração das chaves estrangeiras
            builder.HasOne(l => l.Autor)
                .WithMany()
                .HasForeignKey(l => l.AutorId)
                .HasConstraintName("FK_Livros_Autores_AutorId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Genero)
                .WithMany()
                .HasForeignKey(l => l.GeneroId)
                .HasConstraintName("FK_Livros_Generos_GeneroId")
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(l => l.AutorId)
                .HasDatabaseName("IX_Livros_AutorId");

            builder.HasIndex(l => l.GeneroId)
                .HasDatabaseName("IX_Livros_GeneroId");

            builder.HasIndex(l => l.Titulo)
                .HasDatabaseName("IX_Livros_Titulo");
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=livros;Username=postgres;Password=postgres");
        }
    }
}