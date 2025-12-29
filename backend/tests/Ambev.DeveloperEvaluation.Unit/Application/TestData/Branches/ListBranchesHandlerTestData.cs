using Ambev.DeveloperEvaluation.Application.Queries.Branches.SearchBranches;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Branches;

/// <summary>
/// Provides helper methods for generating test data for ListBranchesHandler.
/// </summary>
public static class ListBranchesHandlerTestData
{
    public static SearchBranchesQuery GenerateValidCommand()
    {
        return new SearchBranchesQuery(
            page: 1,
            pageSize: 10,
            orderBy: "Name"
        );
    }
}
