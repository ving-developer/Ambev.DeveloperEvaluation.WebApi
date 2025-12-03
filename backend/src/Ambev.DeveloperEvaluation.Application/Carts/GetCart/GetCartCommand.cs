using Ambev.DeveloperEvaluation.Application.Carts.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public record GetCartCommand(Guid CartId) : IRequest<CartResult>;