using Ambev.DeveloperEvaluation.Application.Common.Carts;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Queries.Carts.GetCartById;

/// <summary>
/// Contains params to get a cart by its Id
/// </summary>
/// <param name="CartId"></param>
public record GetCartByIdQuery(Guid CartId) : IRequest<CartResult>;