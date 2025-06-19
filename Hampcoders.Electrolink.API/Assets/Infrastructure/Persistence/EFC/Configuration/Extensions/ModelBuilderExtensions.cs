using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAssetsConfiguration(this ModelBuilder builder)
    {
        // --- Configuración para ComponentType ---
        builder.Entity<ComponentType>(b =>
        {
            b.ToTable("component_types");
            b.HasKey(ct => ct.Id);
            b.Property(ct => ct.Id).IsRequired().ValueGeneratedOnAdd();
            b.Property(ct => ct.Name).IsRequired().HasMaxLength(50);
            b.Property(ct => ct.Description).HasMaxLength(250);
            b.Property(ct => ct.Id).HasConversion(id => id.Id, value => new ComponentTypeId(value));
        });

        // --- Configuración para Component ---
        builder.Entity<Component>(b =>
        {
            b.ToTable("components");
            b.HasKey(c => c.Id);
            b.Property(c => c.Id).HasConversion(id => id.Id, value => new ComponentId(value));
            b.Property(c => c.Name).IsRequired().HasMaxLength(100);
            b.Property(c => c.Description).HasMaxLength(500);
            b.Property(c => c.IsActive).IsRequired();
            b.Property(c => c.TypeId).HasConversion(id => id.Id, value => new ComponentTypeId(value)).HasColumnName("component_type_id");
            b.HasOne<ComponentType>().WithMany().HasForeignKey(c => c.TypeId).IsRequired();
        });

        // --- Configuración para Property ---
        builder.Entity<Property>(b =>
        {
            b.ToTable("properties");
            b.HasKey(p => p.Id);
            b.Property(p => p.Id).HasConversion(id => id.Id, value => new PropertyId(value));
            b.Property(p => p.OwnerId).HasConversion(id => id.Id, value => new OwnerId(value)).HasColumnName("owner_id");

            b.OwnsOne(p => p.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("address_street").IsRequired().HasMaxLength(120);
                address.Property(a => a.Number).HasColumnName("address_number").HasMaxLength(20);
                address.Property(a => a.City).HasColumnName("address_city").IsRequired().HasMaxLength(50);
                address.Property(a => a.PostalCode).HasColumnName("address_postal_code").HasMaxLength(10);
                address.Property(a => a.Country).HasColumnName("address_country").IsRequired().HasMaxLength(30);
            });
            
            b.OwnsOne(p => p.Region, region =>
            {
                region.Property(r => r.Name).HasColumnName("region_name").IsRequired().HasMaxLength(50);
                region.Property(r => r.Code).HasColumnName("region_code").HasMaxLength(10);
            });
            
            b.OwnsOne(p => p.District, district =>
            {
               district.Property(d => d.Name).HasColumnName("district_name").IsRequired().HasMaxLength(50);
               district.Property(d => d.Ubigeo).HasColumnName("district_ubigeo").HasMaxLength(10);
            });

            // --- INICIO DE LA SOLUCIÓN FINAL ---
            // Se configura la colección de Photos como un tipo poseído (Owned Type).
            b.OwnsOne(p => p.Photo, photoBuilder =>
            {
                // Mapeamos la propiedad del Value Object a una columna en la tabla 'properties'.
                // Como la propiedad 'Photo' puede ser nula, la columna también debe serlo.
                photoBuilder.Property(photo => photo.PhotoURL)
                    .HasColumnName("photo_url") // Nombre de la columna
                    .HasMaxLength(500)
                    .IsRequired(false); // Se permite que sea nulo
            });
        });
        
        builder.Entity<TechnicianInventory>(b =>
        {
            b.ToTable("technician_inventories");
            b.HasKey(i => i.Id);
            b.Property(i => i.Id).IsRequired().ValueGeneratedNever();  // CORRECCIÓN: Indicamos que es un Id generado por la aplicación
    
            b.Property(i => i.TechnicianId)
                .HasConversion(id => id.Id, value => new TechnicianId(value))
                .IsRequired();
            b.HasIndex(i => i.TechnicianId).IsUnique();
    
            b.HasMany(i => i.StockItems)
                .WithOne()
                .HasForeignKey(cs => cs.TechnicianInventoryId)
                .IsRequired();
        });

        builder.Entity<ComponentStock>(b =>
        {
            b.ToTable("component_stocks");
            b.HasKey(cs => cs.Id);

            b.HasIndex(cs => new { cs.TechnicianInventoryId, cs.ComponentId }).IsUnique();

            b.HasOne<Component>()
                .WithMany()
                .HasForeignKey(cs => cs.ComponentId)
                .IsRequired();

            b.Property(cs => cs.TechnicianInventoryId).IsRequired();
            b.Property(cs => cs.ComponentId).HasConversion(id => id.Id, value => new ComponentId(value));
            b.Property(cs => cs.QuantityAvailable).IsRequired();
            b.Property(cs => cs.AlertThreshold).IsRequired();
            b.Property(cs => cs.LastUpdated).IsRequired();
        });
        // NOTA: No hay ningún builder.Entity<PropertyPhoto>() aquí. Fue eliminado.
    }
}