using Microsoft.EntityFrameworkCore;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Entities;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyServiceDesignAndPlanningConfiguration(this ModelBuilder builder)
{
    builder.Entity<Service>(b =>
    {
        b.ToTable("services"); // <-- AÑADIDO
        b.HasKey(s => s.ServiceId);
        b.Property(s => s.Name).IsRequired().HasMaxLength(80);

        b.OwnsOne(s => s.Policy);
        b.OwnsOne(s => s.Restriction);

        b.OwnsMany(s => s.Tags, a =>
        {
            a.ToTable("service_tags"); // <-- AÑADIDO
            a.WithOwner().HasForeignKey("ServiceId");
            a.Property<int>("Id").ValueGeneratedOnAdd();
            a.HasKey("ServiceId", "Id");
        });

        b.OwnsMany(s => s.Components, a =>
        {
            a.ToTable("service_components"); // <-- AÑADIDO
            a.WithOwner().HasForeignKey("ServiceId");
            a.Property<int>("Id").ValueGeneratedOnAdd();
            a.HasKey("ServiceId", "Id");
        });

        b.OwnsMany(s => s.Plans, a =>
        {
            a.ToTable("service_plans"); // <-- AÑADIDO
            a.WithOwner().HasForeignKey("ServiceId");
            a.Property<int>("Id").ValueGeneratedOnAdd();
            a.HasKey("ServiceId", "Id");
        });

        b.OwnsMany(s => s.Documents, a =>
        {
            a.ToTable("service_documents"); // <-- AÑADIDO
            a.WithOwner().HasForeignKey("ServiceId");
            a.Property<int>("Id").ValueGeneratedOnAdd();
            a.HasKey("ServiceId", "Id");
        });
    });

    builder.Entity<Request>(b =>
    {
        b.ToTable("requests"); // <-- AÑADIDO
        b.HasKey(r => r.RequestId);
        b.OwnsOne(r => r.Bill);

        b.OwnsMany(r => r.Photos, a =>
        {
            a.ToTable("request_photos"); // <-- AÑADIDO
            a.WithOwner().HasForeignKey("RequestId");
            a.Property<int>("Id").ValueGeneratedOnAdd();
            a.HasKey("RequestId", "Id");
        });
    });

    builder.Entity<Schedule>(b =>
    {
        b.ToTable("schedules"); // <-- AÑADIDO
        b.HasKey(s => s.ScheduleId);
    });
}
}
