using Ambev.DeveloperEvaluation.Application.Common.Products;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Queries.Products.GetProductById;

/// <summary>
/// Handler for processing GetProductByIdQuery requests
/// </summary>
public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductByIdHandler> _logger;

    /// <summary>
    /// Initializes a new instance of GetProductHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public GetProductByIdHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<GetProductByIdHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the GetProductCommand request
    /// </summary>
    /// <param name="request">The GetProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product details if found</returns>
    public async Task<ProductResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[INF] Starting GetProductHandler for ProductId={ProductId}", request.Id);

        var product = await _productRepository.GetByIdAsync(
            request.Id,
            cancellationToken,
            p => p.Rating
        );

        if (product == null)
        {
            _logger.LogWarning("[WRN] Product not found. ProductId={ProductId}", request.Id);
            throw new EntityNotFoundException(nameof(Product), request.Id);
        }

        _logger.LogInformation("[INF] Product retrieved successfully. ProductId={ProductId}", request.Id);

        return _mapper.Map<ProductResult>(product);
    }
}
