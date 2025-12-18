using Ambev.DeveloperEvaluation.Application.Queries.Users.GetUserById;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;

/// <summary>
/// Profile for mapping GetUser feature requests to commands
/// </summary>
public class GetUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUser feature
    /// </summary>
    public GetUserProfile()
    {
        CreateMap<Guid, GetUserByIdQuery>()
            .ConstructUsing(id => new GetUserByIdQuery(id));
    }
}
