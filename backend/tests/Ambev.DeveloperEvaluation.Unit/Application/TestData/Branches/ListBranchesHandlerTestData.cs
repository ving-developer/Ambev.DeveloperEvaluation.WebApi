using Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Branches;

/// <summary>
/// Provides helper methods for generating test data for ListBranchesHandler.
/// </summary>
public static class ListBranchesHandlerTestData
{
    public static ListBranchesCommand GenerateValidCommand()
    {
        return new ListBranchesCommand(
            page: 1,
            pageSize: 10,
            orderBy: "Name"
        );
    }
}
