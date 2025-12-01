using Ambev.DeveloperEvaluation.Integration.Shared.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Shared.Helpers;

/// <summary>
/// Base class for integration tests.
/// Provides helper utilities for resolving services and
/// instantiating controllers through DI.
/// </summary>
public abstract class IntegrationTestBase : IClassFixture<IntegrationTestFactory>
{
    protected readonly IServiceProvider Services;

    protected IntegrationTestBase(IntegrationTestFactory factory)
    {
        Services = factory.Services;
    }

    /// <summary>
    /// Creates a scoped instance of a controller using DI.
    /// </summary>
    protected TController CreateController<TController>()
    {
        var scope = Services.CreateScope();
        return ActivatorUtilities.CreateInstance<TController>(scope.ServiceProvider);
    }
}

