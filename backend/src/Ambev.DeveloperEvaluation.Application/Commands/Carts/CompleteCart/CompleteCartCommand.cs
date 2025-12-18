using Ambev.DeveloperEvaluation.Application.Common.Carts;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts.CompleteCart;

public record CompleteCartCommand(Guid CartId) : IRequest<CartResult>;