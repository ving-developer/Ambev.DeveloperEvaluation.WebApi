using Ambev.DeveloperEvaluation.Application.Branches.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// Profile for mapping between Branch entity and CreateBranchResponse
/// </summary>
public class CreateBranchProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateBranch operation
    /// </summary>
    public CreateBranchProfile()
    {
        CreateMap<CreateBranchCommand, Branch>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<Branch, BranchResult>();
    }
}
