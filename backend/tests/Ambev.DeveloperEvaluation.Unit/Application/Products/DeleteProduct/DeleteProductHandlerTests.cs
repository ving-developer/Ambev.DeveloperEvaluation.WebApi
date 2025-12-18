using Ambev.DeveloperEvaluation.Application.Commands.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.DeleteProduct
{
    public class DeleteProductHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly DeleteProductHandler _handler;

        public DeleteProductHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new DeleteProductHandler(
                _productRepository,
                Substitute.For<ILogger<DeleteProductHandler>>());
        }

        [Fact(DisplayName = "Delete product successfully → returns Success=true")]
        public async Task Handle_ValidCommand_ReturnsSuccess()
        {
            // Given
            var command = new DeleteProductCommand(Guid.NewGuid());

            _productRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            _productRepository.SaveChangesAsync(Arg.Any<CancellationToken>())
                .Returns(true);

            // When
            var result = await _handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();

            await _productRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
            await _productRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Delete product fails → throws EntityNotFoundException")]
        public async Task Handle_DeleteFails_ThrowsEntityNotFoundException()
        {
            // Given
            var command = new DeleteProductCommand(Guid.NewGuid());

            _productRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            _productRepository.SaveChangesAsync(Arg.Any<CancellationToken>())
                .Returns(false);

            // When
            Func<Task> act = async () =>
                await _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should()
                .ThrowAsync<EntityNotFoundException>();

            await _productRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
            await _productRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
