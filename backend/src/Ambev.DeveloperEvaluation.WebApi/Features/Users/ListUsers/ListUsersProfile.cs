using Ambev.DeveloperEvaluation.Application.Queries.Users.SearchUsers;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;

/// <summary>
/// Profile for mapping ListUsers feature requests to commands
/// </summary>
public class ListUsersProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListUsers operation
    /// </summary>
    public ListUsersProfile()
    {
        CreateMap<ListUsersRequest, SearchUsersQuery>()
            .ForCtorParam("page", opt => opt.MapFrom(src => src.Page))
            .ForCtorParam("pageSize", opt => opt.MapFrom(src => src.Size))
            .ForCtorParam("orderBy", opt => opt.MapFrom(src => src.Order));
    }
}
