using Ambev.DeveloperEvaluation.Application.Common.Branches;
using Ambev.DeveloperEvaluation.Application.Queries.Branches.SearchBranches;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Branches;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches.ListBranches
{
    /// <summary>
    /// Contains unit tests for the <see cref="SearchBranchesHandler"/> class.
    /// </summary>
    public class ListBranchesHandlerTests
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;
        private readonly SearchBranchesHandler _handler;

        public ListBranchesHandlerTests()
        {
            _branchRepository = Substitute.For<IBranchRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new SearchBranchesHandler(
                _branchRepository,
                _mapper,
                Substitute.For<ILogger<SearchBranchesHandler>>());
        }

        [Fact(DisplayName = "Given valid request When listing branches Then returns paginated result")]
        public async Task Handle_ValidRequest_ReturnsPaginatedResult()
        {
            // Given
            var command = ListBranchesHandlerTestData.GenerateValidCommand();
            var branches = BranchTestData.GenerateList(3);
            var paginated = new PaginatedResponse<Branch>(
                branches,
                currentPage: 1,
                totalPages: 1,
                totalCount: 3
            );

            var mappedList = branches.ConvertAll(b => new BranchResult
            {
                Id = b.Id,
                Name = b.Name,
                Code = b.Code,
                City = b.City,
                State = b.State
            });

            _branchRepository.GetPaginatedAsync(
                Arg.Any<System.Linq.Expressions.Expression<Func<Branch, bool>>>(),
                command.Page,
                command.PageSize,
                command.OrderBy,
                Arg.Any<CancellationToken>())
            .Returns(paginated);

            _mapper.Map<List<BranchResult>>(branches).Returns(mappedList);

            // When
            var result = await _handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(3);
            result.CurrentPage.Should().Be(1);
            result.TotalPages.Should().Be(1);
            result.TotalCount.Should().Be(3);

            await _branchRepository.Received(1).GetPaginatedAsync(
                Arg.Any<System.Linq.Expressions.Expression<Func<Branch, bool>>>(),
                command.Page,
                command.PageSize,
                command.OrderBy,
                Arg.Any<CancellationToken>());

            _mapper.Received(1).Map<List<BranchResult>>(branches);
        }

        [Fact(DisplayName = "Given empty database When listing branches Then returns empty result")]
        public async Task Handle_NoBranches_ReturnsEmptyList()
        {
            // Given
            var command = ListBranchesHandlerTestData.GenerateValidCommand();

            var paginated = new PaginatedResponse<Branch>(
                [],
                currentPage: 1,
                totalPages: 0,
                totalCount: 0
            );

            _branchRepository.GetPaginatedAsync(
                Arg.Any<System.Linq.Expressions.Expression<Func<Branch, bool>>>(),
                command.Page,
                command.PageSize,
                command.OrderBy,
                Arg.Any<CancellationToken>())
            .Returns(paginated);

            _mapper.Map<List<BranchResult>>(Arg.Any<List<Branch>>()).Returns(new List<BranchResult>());

            // When
            var result = await _handler.Handle(command, CancellationToken.None);

            // Then
            result.Data.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            result.TotalPages.Should().Be(0);

            await _branchRepository.Received(1).GetPaginatedAsync(
                Arg.Any<System.Linq.Expressions.Expression<Func<Branch, bool>>>(),
                command.Page,
                command.PageSize,
                command.OrderBy,
                Arg.Any<CancellationToken>());
        }
    }
}
