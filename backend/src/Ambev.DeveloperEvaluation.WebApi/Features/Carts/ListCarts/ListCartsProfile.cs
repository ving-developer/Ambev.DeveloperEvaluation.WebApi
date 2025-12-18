using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.SearchCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;

public class ListCartsProfile : Profile
{
    public ListCartsProfile()
    {
        CreateMap<ListCartsRequest, SearchCartsQuery>()
            .ForCtorParam("page", opt => opt.MapFrom(src => src.Page))
            .ForCtorParam("pageSize", opt => opt.MapFrom(src => src.Size))
            .ForCtorParam("orderBy", opt => opt.MapFrom(src => src.Order));

        CreateMap<CartResult, CartResponse>();
        CreateMap<CartProductResult, CartItem>();
    }
}