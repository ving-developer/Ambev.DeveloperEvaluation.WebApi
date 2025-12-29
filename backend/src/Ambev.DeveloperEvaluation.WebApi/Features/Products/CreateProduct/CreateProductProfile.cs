using Ambev.DeveloperEvaluation.Application.Commands.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Common.Products;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

/// <summary>
/// Profile for mapping between Application and API CreateUser responses
/// </summary>
public class CreateProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser feature
    /// </summary>
    public CreateProductProfile()
    {
        CreateMap<CreateProductRequest, CreateProductCommand>();
        CreateMap<ProductRating, ProductRatingResponse>().ReverseMap();
        CreateMap<ProductResult, ProductResponse>();
    }
}
