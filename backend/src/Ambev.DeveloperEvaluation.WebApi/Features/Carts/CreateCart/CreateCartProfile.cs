using Ambev.DeveloperEvaluation.Application.Commands.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

public class CreateCartProfile : Profile
{
    public CreateCartProfile()
    {
        CreateMap<CartItem, CartItemCommand>();
        CreateMap<CreateCartRequest, CreateCartCommand>();
        CreateMap<CartItemCommand, CartItem>();
        CreateMap<CartResult, CartResponse>();
    }
}