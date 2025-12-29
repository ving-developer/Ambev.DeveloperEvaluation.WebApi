using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.SearchCarts;
using Ambev.DeveloperEvaluation.Domain.Queries.Carts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.SearchCarts;

public class SearchCartsProfile : Profile
{
    public SearchCartsProfile()
    {
        CreateMap<SearchCartsRequest, SearchCartsQuery>()
            .ForCtorParam("page", opt => opt.MapFrom(src => src.Page))
            .ForCtorParam("pageSize", opt => opt.MapFrom(src => src.Size))
            .ForCtorParam("orderBy", opt => opt.MapFrom(src => src.Order));

        CreateMap<CartProductResult, CartItem>();
        CreateMap<SearchCartsQuery, CartSearchFilter>();
    }
}