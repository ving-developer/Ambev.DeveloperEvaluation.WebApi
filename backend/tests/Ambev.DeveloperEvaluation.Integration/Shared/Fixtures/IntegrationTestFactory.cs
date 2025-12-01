using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Integration.Shared.Constants;
using Ambev.DeveloperEvaluation.Integration.Shared.TestData.Users;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Shared.Fixtures;

/// <summary>
/// Provides the infrastructure required to run integration tests for the Web API.
/// This factory sets up an isolated environment using a PostgreSQL Testcontainer,
/// overrides the application's database configuration, applies migrations,
/// and seeds deterministic initial data.
///
/// It ensures that each test suite executes against a fully functional and
/// realistic environment, without relying on external databases.
/// </summary>
public class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationTestFactory"/> class
    /// and configures a PostgreSQL Testcontainer used exclusively for integration tests.
    /// </summary>
    public IntegrationTestFactory()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    /// <summary>
    /// Starts the PostgreSQL Testcontainer before any tests are executed.
    /// Executed once per test suite through <see cref="IAsyncLifetime"/>.
    /// </summary>
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    /// <summary>
    /// Stops the PostgreSQL Testcontainer after all tests have been executed.
    /// Ensures proper cleanup of resources.
    /// </summary>
    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }

    /// <summary>
    /// Configures the WebHost for integration tests.
    /// Overrides the real database context to use the Testcontainer PostgreSQL instance,
    /// applies migrations, and seeds deterministic initial data
    /// (such as the initial authentication user).
    /// </summary>
    /// <param name="builder">The WebHost builder used for configuring the test server.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext configuration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // Replace with PostgreSQL Testcontainer
            services.AddDbContext<DefaultContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });

            // Build provider so we can run migrations and seed data
            var provider = services.BuildServiceProvider();

            using var scope = provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();

            db.Database.Migrate();
            SeedData(db);
        });
    }

    /// <summary>
    /// Seeds deterministic initial data into the test database.
    /// This includes creating the initial user used across integration tests,
    /// ensuring predictable authentication behavior.
    /// </summary>
    /// <param name="db">The test database context.</param>
    private static void SeedData(DefaultContext db)
    {
        if (!db.Users.Any())
        {
            var hasher = new BCryptPasswordHasher();
            var user = UserTestData.GetValidUser();

            user.Email = IntegrationTestConstants.InitialUserEmail;
            user.Password = hasher.HashPassword(IntegrationTestConstants.InitialUserPassword);

            db.Users.Add(user);

            db.SaveChanges();
        }
    }
}
