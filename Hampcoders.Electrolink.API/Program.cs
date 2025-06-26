using Hamcoders.Electrolink.API.Monitoring.Application.Internal.CommandServices;
using Hamcoders.Electrolink.API.Monitoring.Application.Internal.QueryServices;
using Hamcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hamcoders.Electrolink.API.Monitoring.Domain.Services;
using Hamcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EfCore;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;

using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Repositories;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Services;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Infrastructure.Persistence.EFC.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString == null) throw new InvalidOperationException("Connection string not found");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UseNpgsql(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    else if (builder.Environment.IsProduction())
        options.UseNpgsql(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error);
});


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Hampcoders.ElectrolinkPlatform.API",
            Version = "v1",
            Description = "Hampcoders Electrolink Platform API",
            Contact = new OpenApiContact
            {
                Name = "Hampcoders",
                Email = "contact@hampcoders.com"
            },
            License = new OpenApiLicense
            {
                Name = "Apache 2.0",
                Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0")
            },
        });
    options.EnableAnnotations();
    options.AddServer(new OpenApiServer
    {
        Url = "http://localhost:5055", 
        Description = "Development Server"
    });
});

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", 
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod().AllowAnyHeader());
});

// Dependency Injection
// Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IServiceOperationRepository, ServiceOperationRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
// Domain services for Monitoring
builder.Services.AddScoped<IServiceOperationCommandService, ServiceOperationCommandService>();
builder.Services.AddScoped<IServiceOperationQueryService, ServiceOperationQueryService>();
builder.Services.AddScoped<IReportCommandService, ReportCommandService>();
builder.Services.AddScoped<IReportQueryService, ReportQueryService>();
builder.Services.AddScoped<IRatingCommandService, RatingCommandService>();
builder.Services.AddScoped<IRatingQueryService, RatingQueryService>();

// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Assets Bounded Context - Repositories
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<ITechnicianInventoryRepository, TechnicianInventoryRepository>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<IComponentTypeRepository, ComponentTypeRepository>();

// Assets Bounded Context - Command Services
builder.Services.AddScoped<IPropertyCommandService, PropertyCommandService>();
builder.Services.AddScoped<ITechnicianInventoryCommandService, TechnicianInventoryCommandService>();
builder.Services.AddScoped<IComponentCommandService, ComponentCommandService>();
builder.Services.AddScoped<IComponentTypeCommandService, ComponentTypeCommandService>();

// Assets Bounded Context - Query Services
builder.Services.AddScoped<IPropertyQueryService, PropertyQueryService>();
builder.Services.AddScoped<ITechnicianInventoryQueryService, TechnicianInventoryQueryService>();
builder.Services.AddScoped<IComponentQueryService, ComponentQueryService>();
builder.Services.AddScoped<IComponentTypeQueryService, ComponentTypeQueryService>();
// SDP Bounded Context
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();

builder.Services.AddScoped<IServiceCommandService, ServiceCommandService>();
builder.Services.AddScoped<IServiceQueryService, ServiceQueryService>();
builder.Services.AddScoped<IRequestCommandService, RequestCommandService>();
builder.Services.AddScoped<IRequestQueryService, RequestQueryService>();
builder.Services.AddScoped<IScheduleCommandService, ScheduleCommandService>();
builder.Services.AddScoped<IScheduleQueryService, ScheduleQueryService>();
// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAllPolicy");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Urls.Add("http://*:8080");

app.Run();
