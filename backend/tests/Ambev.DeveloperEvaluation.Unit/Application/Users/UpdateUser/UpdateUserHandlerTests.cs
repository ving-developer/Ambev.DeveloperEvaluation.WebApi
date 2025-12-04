using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Users;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users.UpdateUser;

public class UpdateUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly UpdateUserHandler _handler;

    public UpdateUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UpdateUserCommand, User>();
            cfg.CreateMap<User, UserResult>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Username));
        });

        _mapper = mapperConfig.CreateMapper();

        _handler = new UpdateUserHandler(
            _userRepository,
            _mapper,
            Substitute.For<ILogger<UpdateUserHandler>>());
    }

    [Fact(DisplayName = "Update user successfully → returns updated UserResult")]
    public async Task Handle_ValidRequest_ReturnsUpdatedUser()
    {
        // Given
        var existingUser = UserTestData.GenerateValidUser();
        existingUser.Id = Guid.NewGuid();

        var command = UpdateUserHandlerTestData.GenerateValidCommand()
            with
        { Id = existingUser.Id };

        _userRepository.GetByIdAsync(existingUser.Id, Arg.Any<CancellationToken>())
            .Returns(existingUser);

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        _userRepository.UpdateAsync(existingUser, Arg.Any<CancellationToken>())
            .Returns(existingUser);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Email.Should().Be(command.Email);
        result.Name.Should().Be(command.Username);

        await _userRepository.Received(1).UpdateAsync(existingUser, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "User not found → throws EntityNotFoundException")]
    public async Task Handle_UserNotFound_ThrowsEntityNotFoundException()
    {
        // Given
        var command = UpdateUserHandlerTestData.GenerateValidCommand();

        _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // When
        Func<Task> act = async () =>
            await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact(DisplayName = "Email already in use → throws DomainException")]
    public async Task Handle_EmailAlreadyInUse_ThrowsDomainException()
    {
        // Given
        var existingUser = UserTestData.GenerateValidUser();
        existingUser.Id = Guid.NewGuid();

        var conflictingUser = UserTestData.GenerateValidUser();
        conflictingUser.Id = Guid.NewGuid();

        var command = UpdateUserHandlerTestData.GenerateValidCommand();

        _userRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(existingUser);

        _userRepository.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(conflictingUser);

        // When
        Func<Task> act = async () =>
            await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage($"Email {command.Email} is already in use by another user");
    }

    [Fact(DisplayName = "Mapper should update properties in existing entity")]
    public async Task Handle_MapperUpdatesProperties()
    {
        // Given
        var existingUser = UserTestData.GenerateValidUser();
        existingUser.Id = Guid.NewGuid();

        var command = UpdateUserHandlerTestData.GenerateValidCommand()
            with
        { Id = existingUser.Id };

        _userRepository.GetByIdAsync(existingUser.Id, Arg.Any<CancellationToken>())
            .Returns(existingUser);

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        _userRepository.UpdateAsync(existingUser, Arg.Any<CancellationToken>())
            .Returns(existingUser);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        existingUser.Username.Should().Be(command.Username);
        existingUser.Email.Should().Be(command.Email);
        existingUser.Phone.Should().Be(command.Phone);

        result.Email.Should().Be(command.Email);
    }
}
