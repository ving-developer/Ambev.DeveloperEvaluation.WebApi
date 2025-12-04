using Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches.DeleteBranch
{
    /// <summary>
    /// Contains unit tests for the <see cref="DeleteBranchHandler"/> class.
    /// </summary>
    public class DeleteBranchHandlerTests
    {
        private readonly IBranchRepository _branchRepository;
        private readonly DeleteBranchHandler _handler;

        public DeleteBranchHandlerTests()
        {
            _branchRepository = Substitute.For<IBranchRepository>();
            _handler = new DeleteBranchHandler(
                _branchRepository,
                Substitute.For<ILogger<DeleteBranchHandler>>());
        }

        [Fact(DisplayName = "Given valid branch ID When deleting branch Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Given
            var command = new DeleteBranchCommand(Guid.NewGuid());

            _branchRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            _branchRepository.SaveChangesAsync(Arg.Any<CancellationToken>())
                .Returns(true);

            // When
            var result = await _handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().BeTrue();
            await _branchRepository.Received(1)
                .DeleteAsync(command.Id, Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given valid branch ID When delete fails Then throws EntityNotFoundException")]
        public async Task Handle_DeleteFails_ThrowsException()
        {
            // Given
            var command = new DeleteBranchCommand(Guid.NewGuid());

            _branchRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            _branchRepository.SaveChangesAsync(Arg.Any<CancellationToken>())
                .Returns(false);

            // When
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<EntityNotFoundException>();
        }
    }
}
