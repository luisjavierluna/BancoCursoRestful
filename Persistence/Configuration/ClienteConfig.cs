using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class ClienteConfig : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nombre)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(c => c.Apellido)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(c => c.FechaNacimiento)
                .IsRequired();

            builder.Property(c => c.Telefono)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(100);

            builder.Property(c => c.Direccion)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(p => p.Edad);

            builder.Property(p => p.CreatedBy)
                .HasMaxLength(30);

            builder.Property(p => p.LastModifiedBy)
                .HasMaxLength(30);
        }
    }
}
