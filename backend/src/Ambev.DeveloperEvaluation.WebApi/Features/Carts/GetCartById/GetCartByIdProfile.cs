using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.GetCartById;
using Ambev.DeveloperEvaluation.Domain.ReadModels.Carts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCartById;

public class GetCartByIdProfile : Profile
{
    public GetCartByIdProfile()
    {
        CreateMap<Guid, GetCartByIdQuery>()
            .ConstructUsing(id => new GetCartByIdQuery(id));

        CreateMap<CartReadModel, CartResult>();
        CreateMap<CartResult, CartResponse>();
        CreateMap<CartProductReadModel, CartItemCommand>();
    }
}