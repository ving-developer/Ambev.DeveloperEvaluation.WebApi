using Ambev.DeveloperEvaluation.Application.Branches.Common;
using Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Branches;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches.CreateBranch;

/// <summary>
/// Contains unit tests for the <see cref="CreateBranchHandler"/> class.
/// </summary>
public class CreateBranchHandlerTests
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateBranchHandler> _logger;
    private readonly CreateBranchHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBranchHandlerTests"/> class.
    /// Sets up all dependencies and the handler under test using NSubstitute mocks.
    /// </summary>
    public CreateBranchHandlerTests()
    {
        _branchRepository = Substitute.For<IBranchRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateBranchHandler>>();
        _handler = new CreateBranchHandler(_branchRepository, _mapper, _logger);
    }

    /// <summary>
    /// Tests that a valid branch creation command results in a successful creation.
    /// Verifies that:
    /// - The branch is saved to the repository
    /// - The result contains the expected branch data
    /// - SaveChangesAsync is called
    /// </summary>
    [Fact(DisplayName = "Given valid branch data When creating branch Then returns branch result")]
    public async Task Handle_ValidBranchData_ReturnsBranchResult()
    {
        // Given
        var command = CreateBranchHandlerTestData.GenerateValidCommand();
        var branch = CreateBranchHandlerTestData.GenerateValidBranch();
        var expectedResult = CreateBranchHandlerTestData.GenerateValidBranchResult();

        _branchRepository.GetByCodeAsync(command.Code, Arg.Any<CancellationToken>()).Returns((Branch)null);
        _mapper.Map<Branch>(command).Returns(branch);
        _branchRepository.CreateAsync(branch, Arg.Any<CancellationToken>()).Returns(branch);
        _mapper.Map<BranchResult>(branch).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);

        await _branchRepository.Received(1).GetByCodeAsync(command.Code, Arg.Any<CancellationToken>());
        await _branchRepository.Received(1).CreateAsync(branch, Arg.Any<CancellationToken>());
        await _branchRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        _mapper.Received(1).Map<Branch>(command);
        _mapper.Received(1).Map<BranchResult>(branch);
    }

    /// <summary>
    /// Tests that attempting to create a branch with an existing code throws an <see cref="InvalidOperationException"/>.
    /// Verifies that:
    /// - The exception is thrown with the correct message
    /// - The branch is not created
    /// </summary>
    [Fact(DisplayName = "Given existing branch code When creating branch Then throws InvalidOperationException")]
    public async Task Handle_ExistingBranchCode_ThrowsInvalidOperationException()
    {
        // Given
        var command = CreateBranchHandlerTestData.GenerateValidCommand();
        var existingBranch = CreateBranchHandlerTestData.GenerateValidBranch();

        _branchRepository.GetByCodeAsync(command.Code, Arg.Any<CancellationToken>()).Returns(existingBranch);

        // When
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Branch with code {command.Code} already exists");

        await _branchRepository.Received(1).GetByCodeAsync(command.Code, Arg.Any<CancellationToken>());
        await _branchRepository.DidNotReceive().CreateAsync(Arg.Any<Branch>(), Arg.Any<CancellationToken>());
        await _branchRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the handler properly handles repository exceptions during creation.
    /// Verifies that:
    /// - Exceptions from repository are propagated
    /// </summary>
    [Fact(DisplayName = "Given repository error When creating branch Then propagates exception")]
    public async Task Handle_RepositoryError_PropagatesException()
    {
        // Given
        var command = CreateBranchHandlerTestData.GenerateValidCommand();
        var branch = CreateBranchHandlerTestData.GenerateValidBranch();
        var expectedException = new Exception("Database connection failed");

        _branchRepository.GetByCodeAsync(command.Code, Arg.Any<CancellationToken>()).Returns((Branch)null);
        _mapper.Map<Branch>(command).Returns(branch);
        _branchRepository.CreateAsync(branch, Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        // When
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        var exception = await act.Should().ThrowAsync<Exception>();
        exception.WithMessage("Database connection failed");
    }

    /// <summary>
    /// Tests that the handler properly handles repository exceptions during code check.
    /// Verifies that:
    /// - Exceptions from GetByCodeAsync are propagated
    /// </summary>
    [Fact(DisplayName = "Given repository error when checking code When creating branch Then propagates exception")]
    public async Task Handle_RepositoryErrorOnCodeCheck_PropagatesException()
    {
        // Given
        var command = CreateBranchHandlerTestData.GenerateValidCommand();
        var expectedException = new Exception("Database timeout");

        _branchRepository.GetByCodeAsync(command.Code, Arg.Any<CancellationToken>()).ThrowsAsync(expectedException);

        // When
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        var exception = await act.Should().ThrowAsync<Exception>();
        exception.WithMessage("Database timeout");
    }

    /// <summary>
    /// Tests that the handler properly handles AutoMapper exceptions.
    /// Verifies that:
    /// - Exceptions from AutoMapper are propagated
    /// </summary>
    [Fact(DisplayName = "Given AutoMapper error When creating branch Then propagates exception")]
    public async Task Handle_AutoMapperError_PropagatesException()
    {
        // Given
        var command = CreateBranchHandlerTestData.GenerateValidCommand();
        var expectedException = new AutoMapperMappingException("Mapping failed");

        _branchRepository.GetByCodeAsync(command.Code, Arg.Any<CancellationToken>()).Returns((Branch)null);
        _mapper.Map<Branch>(command).Throws(expectedException);

        // When
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<AutoMapperMappingException>()
            .WithMessage("Mapping failed");
    }
}