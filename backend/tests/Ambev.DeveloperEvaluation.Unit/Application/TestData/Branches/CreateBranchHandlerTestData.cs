using Ambev.DeveloperEvaluation.Application.Commands.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.Application.Common.Branches;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Branches;

/// <summary>
/// Test data generator for CreateBranch tests using Bogus
/// </summary>
public static class CreateBranchHandlerTestData
{
    private static readonly Faker<CreateBranchCommand> _commandFaker = new Faker<CreateBranchCommand>("pt_BR")
        .RuleFor(c => c.Code, f => $"BR{f.Random.Number(100, 999):000}")
        .RuleFor(c => c.Name, f => $"Filial {f.Address.City()}")
        .RuleFor(c => c.City, f => f.Address.City())
        .RuleFor(c => c.State, f => f.Address.StateAbbr());

    private static readonly Faker<Branch> _branchFaker = new Faker<Branch>("pt_BR")
        .CustomInstantiator(f => new Branch(
            $"Filial {f.Address.City()}",
            $"BR{f.Random.Number(100, 999):000}",
            f.Address.City(),
            f.Address.StateAbbr()
        ))
        .RuleFor(b => b.Id, (f, b) => f.Random.Guid());

    private static readonly Faker<BranchResult> _resultFaker = new Faker<BranchResult>("pt_BR")
        .RuleFor(r => r.Id, f => f.Random.Guid())
        .RuleFor(r => r.Code, f => $"BR{f.Random.Number(100, 999):000}")
        .RuleFor(r => r.Name, f => $"Filial {f.Address.City()}")
        .RuleFor(r => r.City, f => f.Address.City())
        .RuleFor(r => r.State, f => f.Address.StateAbbr());

    /// <summary>
    /// Generates a valid CreateBranchCommand with random data
    /// </summary>
    public static CreateBranchCommand GenerateValidCommand()
    {
        return _commandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid Branch with random data
    /// </summary>
    public static Branch GenerateValidBranch()
    {
        return _branchFaker.Generate();
    }

    /// <summary>
    /// Generates a valid BranchResult with random data
    /// </summary>
    public static BranchResult GenerateValidBranchResult()
    {
        return _resultFaker.Generate();
    }
}