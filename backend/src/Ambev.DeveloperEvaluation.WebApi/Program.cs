using Ambev.DeveloperEvaluation.WebApi.Extensions;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureLogging();
            builder.ConfigureServices();

            var app = builder.Build();

            app.ConfigureMiddleware();
            app.ConfigureEndpoints();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
