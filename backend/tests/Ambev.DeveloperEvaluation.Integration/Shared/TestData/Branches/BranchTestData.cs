using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Integration.Shared.Constants;

namespace Ambev.DeveloperEvaluation.Integration.Shared.TestData.Branches;

internal static class BranchTestData
{
    /// <summary>
    /// Generates an initial Branch entity with predefined constants for integration tests.
    /// This branch has fixed data as specified in IntegrationTestConstants.
    /// </summary>
    public static Branch GetInitialBranch()
    {
        return new Branch(
            name: IntegrationTestConstants.InitialBranchName,
            code: IntegrationTestConstants.InitialBranchCode,
            city: IntegrationTestConstants.InitialBranchCity,
            state: IntegrationTestConstants.InitialBranchState)
        {
            Id = IntegrationTestConstants.InitialBranchId
        };
    }
}