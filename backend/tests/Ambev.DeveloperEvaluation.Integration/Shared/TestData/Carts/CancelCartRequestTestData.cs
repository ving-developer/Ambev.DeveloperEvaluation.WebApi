using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CancelCart;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Shared.TestData.Carts;

/// <summary>
/// Provides methods for generating test data for CancelCartRequest.
/// </summary>
internal static class CancelCartRequestTestData
{
    /// <summary>
    /// Generates a valid CancelCartRequest with a random reason.
    /// </summary>
    public static CancelCartRequest GetValidCancelCartRequest()
    {
        return new Faker<CancelCartRequest>()
            .RuleFor(c => c.Reason, f => f.Lorem.Sentence())
            .Generate();
    }

    /// <summary>
    /// Generates an invalid CancelCartRequest with an empty reason.
    /// </summary>
    public static CancelCartRequest GetInvalidCancelCartRequest()
    {
        return new CancelCartRequest
        {
            Reason = string.Empty
        };
    }
}
