using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dominio.Entidades;

namespace Infra.Mapeamento;

public class AutorMapeamento : IEntityTypeConfiguration<Autor>
{
    public void Configure(EntityTypeBuilder<Autor> builder)
    {
        builder.Property(a => a.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(a => a.Nome);
    }
}