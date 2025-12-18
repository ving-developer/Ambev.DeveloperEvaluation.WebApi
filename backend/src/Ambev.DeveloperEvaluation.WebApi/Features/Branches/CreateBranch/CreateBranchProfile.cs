using Ambev.DeveloperEvaluation.Application.Commands.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.Application.Common.Branches;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;

/// <summary>
/// Profile for mapping between Application and API CreateBranch responses
/// </summary>
public class CreateBranchProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateBranch feature
    /// </summary>
    public CreateBranchProfile()
    {
        CreateMap<CreateBranchRequest, CreateBranchCommand>();
        CreateMap<BranchResult, BranchResponse>();
    }
}
