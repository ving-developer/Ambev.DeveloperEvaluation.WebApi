using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.WebApi.Middleware;

namespace Ambev.DeveloperEvaluation.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ValidationExceptionMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseBasicHealthChecks();
    }

    public static void ConfigureEndpoints(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();
    }
}
