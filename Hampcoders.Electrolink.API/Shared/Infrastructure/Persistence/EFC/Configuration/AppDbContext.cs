using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Application database context
/// </summary>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /*public DbSet<Property> Properties { get; set; }
    public DbSet<TechnicianInventory> TechnicianInventories { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }
    public DbSet<ComponentStock> ComponentStocks { get; set; }*/

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Solo tu configuración de SDP
        builder.ApplyServiceDesignAndPlanningConfiguration();

        builder.UseSnakeCaseNamingConvention();
    }


}