using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Ambev.DeveloperEvaluation.WebApi.Extensions;

/// <summary>
/// Extension methods for configuring WebApplicationBuilder.
/// </summary>
public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Configures default logging for the application.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder instance.</param>
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.AddDefaultLogging();
    }

    /// <summary>
    /// Configures all services required by the application.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder instance.</param>
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        ConfigureApiServices(builder);
        ConfigureDatabaseAndAuthentication(builder);
        ConfigureApplicationDependencies(builder);
        ConfigureMediatR(builder);
        ConfigureHealthChecks(builder);
    }

    /// <summary>
    /// Adds controllers and Swagger for API documentation with JWT support.
    /// </summary>
    private static void ConfigureApiServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
        });
    }

    /// <summary>
    /// Configures database context and JWT authentication.
    /// </summary>
    private static void ConfigureDatabaseAndAuthentication(WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
            )
        );

        builder.Services.AddJwtAuthentication(configuration);
    }

    /// <summary>
    /// Registers IoC dependencies, AutoMapper, and FluentValidation.
    /// </summary>
    private static void ConfigureApplicationDependencies(WebApplicationBuilder builder)
    {
        builder.RegisterDependencies();

        builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
        builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
    }

    /// <summary>
    /// Configures MediatR and pipeline behaviors.
    /// </summary>
    private static void ConfigureMediatR(WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(ApplicationLayer).Assembly,
                typeof(Program).Assembly
            );
        });

        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    /// <summary>
    /// Adds basic health checks to the application.
    /// </summary>
    private static void ConfigureHealthChecks(WebApplicationBuilder builder)
    {
        builder.AddBasicHealthChecks();
    }
}
