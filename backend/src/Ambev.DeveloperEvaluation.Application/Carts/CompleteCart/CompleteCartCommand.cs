using Ambev.DeveloperEvaluation.Application.Carts.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CompleteCart;

public record CompleteCartCommand(Guid CartId) : IRequest<CartResult>;