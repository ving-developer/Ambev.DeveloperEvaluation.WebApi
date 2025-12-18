using Ambev.DeveloperEvaluation.Application.Common.Products;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Queries.Product.GetProductById;

/// <summary>
/// Profile for mapping between Product entity and GetProductResult
/// </summary>
public class GetProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetProduct operation
    /// </summary>
    public GetProductProfile()
    {
        CreateMap<Product, ProductResult>();

        CreateMap<Rating, ProductRating>();
    }
}
