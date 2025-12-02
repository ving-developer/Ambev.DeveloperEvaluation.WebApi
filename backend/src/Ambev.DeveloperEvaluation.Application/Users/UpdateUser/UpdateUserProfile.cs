using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
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
        CreateMap<UpdateUserCommand, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Não atualiza ID da entidade
            .ForMember(dest => dest.Password, opt => opt.Ignore()) // Senha não é atualizada aqui
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Mantém data de criação original
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow)) // Atualiza data de modificação
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                srcMember != null));
    }
}