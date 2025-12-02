using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// Profile for mapping ListUsers feature requests to commands
/// </summary>
public class ListProductsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListUsers operation
    /// </summary>
    public ListProductsProfile()
    {
        CreateMap<ListProductsRequest, ListProductsCommand>()
            .ForCtorParam("page", opt => opt.MapFrom(src => src.Page))
            .ForCtorParam("pageSize", opt => opt.MapFrom(src => src.Size))
            .ForCtorParam("orderBy", opt => opt.MapFrom(src => src.Order));
    }
}
