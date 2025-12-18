using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.GetCartById;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

public class GetCartProfile : Profile
{
    public GetCartProfile()
    {
        CreateMap<Guid, GetCartByIdQuery>()
            .ConstructUsing(id => new GetCartByIdQuery(id));

        CreateMap<CartResult, CartResponse>();
    }
}