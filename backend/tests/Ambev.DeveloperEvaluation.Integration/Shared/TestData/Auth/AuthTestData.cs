using Ambev.DeveloperEvaluation.Integration.Shared.Constants;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUser;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Shared.TestData.Auth;

/// <summary>
/// Provides test data generators for authentication-related requests.
/// Ensures consistent and reusable request creation across tests.
/// </summary>
public static class AuthTestData
{
    /// <summary>
    /// Returns a valid request using the known seeded user credentials.
    /// </summary>
    public static AuthenticateUserRequest GetValidSeededUserRequest()
    {
        return new AuthenticateUserRequest
        {
            Email = IntegrationTestConstants.InitialUserEmail,
            Password = IntegrationTestConstants.InitialUserPassword
        };
    }

    /// <summary>
    /// Generates an invalid request (empty fields, failing validation).
    /// </summary>
    public static AuthenticateUserRequest GetInvalidRequest()
    {
        return new AuthenticateUserRequest
        {
            Email = "",
            Password = ""
        };
    }
}
