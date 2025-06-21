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

// OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
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

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod().AllowAnyHeader());
});

// Shared
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// -------------------
// Assets Bounded Context
// -------------------
/*builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<ITechnicianInventoryRepository, TechnicianInventoryRepository>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<IComponentTypeRepository, ComponentTypeRepository>();

builder.Services.AddScoped<IPropertyCommandService, PropertyCommandService>();
builder.Services.AddScoped<ITechnicianInventoryCommandService, TechnicianInventoryCommandService>();
builder.Services.AddScoped<IComponentCommandService, ComponentCommandService>();
builder.Services.AddScoped<IComponentTypeCommandService, ComponentTypeCommandService>();

builder.Services.AddScoped<IPropertyQueryService, PropertyQueryService>();
builder.Services.AddScoped<ITechnicianInventoryQueryService, TechnicianInventoryQueryService>();
builder.Services.AddScoped<IComponentQueryService, ComponentQueryService>();
builder.Services.AddScoped<IComponentTypeQueryService, ComponentTypeQueryService>();*/

// -------------------
// SDP Bounded Context
// -------------------
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();

builder.Services.AddScoped<IServiceCommandService, ServiceCommandService>();
builder.Services.AddScoped<IServiceQueryService, ServiceQueryService>();
builder.Services.AddScoped<IRequestCommandService, RequestCommandService>();
builder.Services.AddScoped<IRequestQueryService, RequestQueryService>();
builder.Services.AddScoped<IScheduleCommandService, ScheduleCommandService>();
builder.Services.AddScoped<IScheduleQueryService, ScheduleQueryService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
