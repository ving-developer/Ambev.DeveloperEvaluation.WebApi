using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Queries.Carts.GetCartById;

/// <summary>
/// Handler for processing GetCartByIdQuery requests
/// </summary>
public class GetCartByIdHandler : IRequestHandler<GetCartByIdQuery, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCartByIdHandler> _logger;

    public GetCartByIdHandler(
        ICartRepository cartRepository,
        IMapper mapper,
        ILogger<GetCartByIdHandler> logger)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CartResult> Handle(GetCartByIdQuery command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving cart {CartId}", command.CartId);

        var cart = await _cartRepository.GetByIdAsync(command.CartId, cancellationToken);
        if (cart == null)
        {
            _logger.LogWarning("Cart {CartId} not found", command.CartId);
            throw new EntityNotFoundException(nameof(Cart), command.CartId);
        }

        _logger.LogInformation("Cart {CartId} retrieved successfully", command.CartId);

        return _mapper.Map<CartResult>(cart);
    }
}