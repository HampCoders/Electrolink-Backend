using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Analytics.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAnalitycsConfiguration(this ModelBuilder builder)
    {
        
        // Analitycs Content
        builder.Entity<Technician>().HasKey(t => t.Id);
        builder.Entity<Technician>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Technician>().Property(t => t.Name).IsRequired().HasMaxLength(30);
        builder.Entity<Technician>().Property(t => t.Email).IsRequired().HasMaxLength(30);
        
        builder.Entity<Work>().HasKey(w => w.Id);
        builder.Entity<Work>().Property(w => w.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Work>().Property(w => w.Title).IsRequired().HasMaxLength(50);
        builder.Entity<Work>().Property(w => w.Description).IsRequired().HasMaxLength(240);
        
        builder.Entity<Asset>().HasKey(a => a.Id);
        builder.Entity<Asset>().HasDiscriminator<string>("asset_type")
            .HasValue<Asset>("asset_base")
            .HasValue<ImageAsset>("asset_image");
        
        builder.Entity<Asset>().OwnsOne(i => i.AssetIdentifier, ai =>
        {
            ai.WithOwner().HasForeignKey("Id");
            ai.Property(p => p.Identifier).HasColumnName("AssetIdentifier");
        });

        builder.Entity<ImageAsset>().Property(p => p.ImageUri).IsRequired();
    }
    
    
    
}