using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dominio.Entidades;

namespace Infra.Mapeamento;

public class LivroMapeamento : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder.Property(l => l.Titulo)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(l => l.DataAtualizacao)
            .IsRequired(false);

        builder.HasOne(l => l.Autor)
            .WithMany()
            .HasForeignKey(l => l.AutorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Genero)
            .WithMany()
            .HasForeignKey(l => l.GeneroId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(l => l.Titulo);
    }
}