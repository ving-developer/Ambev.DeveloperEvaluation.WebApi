using Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Shared.TestData.Branches
{
    internal static class CreateBranchRequestTestData
    {
        /// <summary>
        /// Generates a valid CreateBranchRequest with randomized valid data.
        /// </summary>
        public static CreateBranchRequest GetValidCreateBranchRequest()
        {
            return new Faker<CreateBranchRequest>()
                .RuleFor(b => b.Name, f => f.Company.CompanyName())
                .RuleFor(b => b.City, f => f.Address.City())
                .RuleFor(b => b.State, f => "SP")
                .RuleFor(b => b.Code, f => f.Random.Replace("SP-###"))
                .Generate();
        }

        /// <summary>
        /// Generates an invalid CreateBranchRequest with empty fields
        /// </summary>
        public static CreateBranchRequest GetInvalidCreateBranchRequest()
        {
            return new CreateBranchRequest
            {
                Name = "",
                City = "",
                State = ""
            };
        }
    }
}
