using Ambev.DeveloperEvaluation.Application.Commands.Users.UpdateUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;

/// <summary>
/// Profile for mapping UpdateUser feature requests to commands
/// </summary>
public class UpdateUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateUser operation
    /// </summary>
    public UpdateUserProfile()
    {
        CreateMap<UpdateUserRequest, UpdateUserCommand>()
            .ForCtorParam("id", opt => opt.MapFrom((src, context) =>
            {
                return Guid.Empty;
            }))
            .ForCtorParam("username", opt => opt.MapFrom(src => src.Username))
            .ForCtorParam("phone", opt => opt.MapFrom(src => src.Phone))
            .ForCtorParam("email", opt => opt.MapFrom(src => src.Email))
            .ForCtorParam("status", opt => opt.MapFrom(src => src.Status))
            .ForCtorParam("role", opt => opt.MapFrom(src => src.Role));
    }
}