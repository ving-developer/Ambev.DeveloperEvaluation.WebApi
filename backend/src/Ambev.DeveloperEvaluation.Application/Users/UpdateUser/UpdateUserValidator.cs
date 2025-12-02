using Ambev.DeveloperEvaluation.Application.Validation;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Validator for UpdateUserCommand that defines validation rules for user update command.
/// </summary>
public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateUserValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Id: Must be a valid, non-empty GUID
    /// - Email: Must be in valid format (using EmailValidator)
    /// - Username: Required, must be between 3 and 50 characters
    /// - Phone: Must match Brazilian phone format (XX) XXXXX-XXXX
    /// - Status: Cannot be set to Unknown, must be a valid enum value
    /// - Role: Cannot be set to None, must be a valid enum value
    /// 
    /// Note: Password validation is intentionally omitted as password updates
    /// should be handled through a separate dedicated endpoint for security reasons.
    /// </remarks>
    public UpdateUserValidator()
    {
        RuleFor(user => user.Id)
            .NotEmpty().WithMessage("User ID is required")
            .NotEqual(Guid.Empty).WithMessage("Invalid user ID");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required")
            .SetValidator(new EmailValidator());

        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters")
            .Matches(@"^[a-zA-Z0-9_.-]+$")
            .WithMessage("Username can only contain letters, numbers, dots, underscores and hyphens");

        RuleFor(user => user.Phone)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone must be in valid international format (+X XXXXXXXXXX)");

        RuleFor(user => user.Status)
            .NotEmpty().WithMessage("Status is required")
            .NotEqual(UserStatus.Unknown).WithMessage("Status cannot be 'Unknown'")
            .IsInEnum().WithMessage("Invalid user status");

        RuleFor(user => user.Role)
            .NotEmpty().WithMessage("Role is required")
            .NotEqual(UserRole.None).WithMessage("Role cannot be 'None'")
            .IsInEnum().WithMessage("Invalid user role");
    }
}