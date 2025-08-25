using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dominio.Entidades;

namespace Infra.Mapeamento;

public class GeneroMapeamento : IEntityTypeConfiguration<Genero>
{
    public void Configure(EntityTypeBuilder<Genero> builder)
    {
        builder.Property(g => g.Nome)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(g => g.Nome)
            .IsUnique();
    }
}