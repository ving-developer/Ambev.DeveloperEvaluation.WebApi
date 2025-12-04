using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Handler for processing DeleteProductCommand requests
/// </summary>
public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DeleteProductHandler> _logger;

    /// <summary>
    /// Initializes a new instance of DeleteProductHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="logger">The logger instance</param>
    public DeleteProductHandler(
        IProductRepository productRepository,
        ILogger<DeleteProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the DeleteProductCommand request
    /// </summary>
    /// <param name="request">The DeleteProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the delete operation</returns>
    public async Task<DeleteProductResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[INF] Starting DeleteProductHandler for ProductId={ProductId}", request.Id);

        await _productRepository.DeleteAsync(request.Id, cancellationToken);

        var success = await _productRepository.SaveChangesAsync(cancellationToken);

        if (!success)
        {
            _logger.LogWarning("[WRN] Delete failed. ProductId={ProductId} not found", request.Id);
            throw new EntityNotFoundException(nameof(Product), request.Id);
        }

        _logger.LogInformation("[INF] Product deleted successfully. ProductId={ProductId}", request.Id);

        return new DeleteProductResponse { Success = true };
    }
}
