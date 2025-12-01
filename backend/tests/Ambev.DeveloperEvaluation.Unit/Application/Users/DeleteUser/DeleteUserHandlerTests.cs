using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Users;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users.DeleteUser
{
    /// <summary>
    /// Contains unit tests for the <see cref="DeleteUserHandler"/> class.
    /// </summary>
    public class DeleteUserHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly DeleteUserHandler _handler;

        public DeleteUserHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _handler = new DeleteUserHandler(_userRepository, Substitute.For<ILogger<DeleteUserHandler>>());
        }

        [Fact(DisplayName = "Given valid user ID When deleting user Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Given
            var command = DeleteUserHandlerTestData.GenerateValidCommand();

            _userRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(true);

            // When
            var response = await _handler.Handle(command, CancellationToken.None);

            // Then
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            await _userRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given invalid command When deleting user Then throws validation exception")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Given
            var command = DeleteUserHandlerTestData.GenerateInvalidCommand();

            // When
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact(DisplayName = "Given non-existing user ID When deleting user Then throws KeyNotFoundException")]
        public async Task Handle_NonExistingUser_ThrowsKeyNotFoundException()
        {
            // Given
            var command = DeleteUserHandlerTestData.GenerateValidCommand();

            _userRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(false);

            // When
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"User with ID {command.Id} not found");

            await _userRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
        }
    }
}
