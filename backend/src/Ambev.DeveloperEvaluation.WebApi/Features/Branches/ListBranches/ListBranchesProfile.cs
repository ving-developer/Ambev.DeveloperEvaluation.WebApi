using Ambev.DeveloperEvaluation.Application.Queries.Branches.SearchBranches;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.ListBranches;

/// <summary>
/// Profile for mapping ListUsers feature requests to commands
/// </summary>
public class ListBranchesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListUsers operation
    /// </summary>
    public ListBranchesProfile()
    {
        CreateMap<ListBranchesRequest, SearchBranchesQuery>()
            .ForCtorParam("page", opt => opt.MapFrom(src => src.Page))
            .ForCtorParam("pageSize", opt => opt.MapFrom(src => src.Size))
            .ForCtorParam("orderBy", opt => opt.MapFrom(src => src.Order));
    }
}
