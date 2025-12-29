using Ambev.DeveloperEvaluation.Application.Common.Branches;
using Ambev.DeveloperEvaluation.Application.Queries.Branches.GetBranchById;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches.GetBranchById;

/// <summary>
/// Contains unit tests for the <see cref="GetBranchByIdHandler"/> class.
/// </summary>
public class GetBranchByIdHandlerTests
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly GetBranchByIdHandler _handler;

    public GetBranchByIdHandlerTests()
    {
        _branchRepository = Substitute.For<IBranchRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetBranchByIdHandler(
            _branchRepository,
            _mapper,
            Substitute.For<ILogger<GetBranchByIdHandler>>());
    }

    [Fact(DisplayName = "Given valid branch ID When getting branch Then returns branch result")]
    public async Task Handle_ValidRequest_ReturnsBranchResult()
    {
        // Given
        var command = new GetBranchByIdQuery(Guid.NewGuid());

        var branch = BranchTestData.GenerateValidBranch();
        branch.Id = command.Id;

        var expectedResult = new BranchResult
        {
            Id = branch.Id,
            Name = branch.Name,
            Code = branch.Code,
            City = branch.City,
            State = branch.State
        };

        _branchRepository.GetByIdAsync(branch.Id, Arg.Any<CancellationToken>())
            .Returns(branch);

        _mapper.Map<BranchResult>(branch)
            .Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(branch.Id);
        result.Name.Should().Be(branch.Name);
        result.City.Should().Be(branch.City);

        await _branchRepository.Received(1)
            .GetByIdAsync(branch.Id, Arg.Any<CancellationToken>());

        _mapper.Received(1).Map<BranchResult>(branch);
    }

    [Fact(DisplayName = "Given non-existing branch ID When getting branch Then throws EntityNotFoundException")]
    public async Task Handle_NonExistingBranch_ThrowsEntityNotFoundException()
    {
        // Given
        var command = new GetBranchByIdQuery(Guid.NewGuid());

        _branchRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Branch)null!);

        // When
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<EntityNotFoundException>();

        await _branchRepository.Received(1)
            .GetByIdAsync(command.Id, Arg.Any<CancellationToken>());

        _mapper.DidNotReceive().Map<BranchResult>(Arg.Any<Branch>());
    }
}
