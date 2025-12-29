using Ambev.DeveloperEvaluation.Application.Commands.Carts.AddItemToCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddItemToCart;

public class CreateCartProfile : Profile
{
    public CreateCartProfile()
    {
        CreateMap<AddItemToCartRequest, AddItemToCartCommand>();
    }
}