using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Supplier.Business.Models;

namespace Supplier.Data.Mappings
{
    public class ClienteMapping : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(c => c.Cpf)
                .IsRequired()
                .HasColumnType("varchar(11)");

            builder.Property(c => c.ValorLimite)
                .IsRequired()
                .HasColumnType("decimal");

            builder.ToTable("Clientes");
        }
    }
}
