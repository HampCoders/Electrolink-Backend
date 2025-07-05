using Hamcoders.Electrolink.API.Monitoring.Application.Internal.CommandServices;
using Hamcoders.Electrolink.API.Monitoring.Application.Internal.QueryServices;
using Hamcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hamcoders.Electrolink.API.Monitoring.Domain.Services;
using Hamcoders.Electrolink.API.Monitoring.Infrastructure.Persistence.EfCore;

using Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Repositories;

using Hampcoders.Electrolink.API.IAM.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.IAM.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.IAM.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.IAM.Domain.Repositories;
using Hampcoders.Electrolink.API.IAM.Domain.Services;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Tokens.JWT.Services;
using Hampcoders.Electrolink.API.IAM.Interfaces.ACL;
using Hampcoders.Electrolink.API.IAM.Interfaces.ACL.Services;

using Hampcoders.Electrolink.API.Profiles.Application.ACL;
using Hampcoders.Electrolink.API.Profiles.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Profiles.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Profiles.Domain.Repositories;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Hampcoders.Electrolink.API.Profiles.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;

using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Repositories;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Services;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Infrastructure.Persistence.EFC.Repositories;

using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// General Config
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString == null) throw new InvalidOperationException("Connection string not found");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UseNpgsql(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    else
        options.UseNpgsql(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error);
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hampcoders.ElectrolinkPlatform.API",
        Version = "v1",
        Description = "Hampcoders Electrolink Platform API",
        Contact = new OpenApiContact { Name = "Hampcoders", Email = "contact@hampcoders.com" },
        License = new OpenApiLicense { Name = "Apache 2.0", Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0") }
    });
    options.EnableAnnotations();

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme } },
            Array.Empty<string>()
        }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Shared
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Monitoring
builder.Services.AddScoped<IServiceOperationRepository, ServiceOperationRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IServiceOperationCommandService, ServiceOperationCommandService>();
builder.Services.AddScoped<IServiceOperationQueryService, ServiceOperationQueryService>();
builder.Services.AddScoped<IReportCommandService, ReportCommandService>();
builder.Services.AddScoped<IReportQueryService, ReportQueryService>();
builder.Services.AddScoped<IRatingCommandService, RatingCommandService>();
builder.Services.AddScoped<IRatingQueryService, RatingQueryService>();

// Assets
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
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
builder.Services.AddScoped<IComponentTypeQueryService, ComponentTypeQueryService>();

// SDP
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IServiceCommandService, ServiceCommandService>();
builder.Services.AddScoped<IServiceQueryService, ServiceQueryService>();
builder.Services.AddScoped<IRequestCommandService, RequestCommandService>();
builder.Services.AddScoped<IRequestQueryService, RequestQueryService>();
builder.Services.AddScoped<IScheduleCommandService, ScheduleCommandService>();
builder.Services.AddScoped<IScheduleQueryService, ScheduleQueryService>();

// Profiles
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();

// IAM
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

var app = builder.Build();

// DB Init
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Urls.Add("http://*:8080");
app.Run();
