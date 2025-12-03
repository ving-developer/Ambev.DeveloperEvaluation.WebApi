using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities;


/// <summary>
/// Represents a user in the system with authentication and profile information.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class User : BaseEntity, IUser
{
    /// <summary>
    /// Gets the user's full name.
    /// Must not be null or empty and should contain both first and last names.
    /// </summary>
    public string Username { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the user's email address.
    /// Must be a valid email format and is used as a unique identifier for authentication.
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the user's phone number.
    /// Must be a valid phone number format following the pattern (XX) XXXXX-XXXX.
    /// </summary>
    public string Phone { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the hashed password for authentication.
    /// Password must meet security requirements: minimum 8 characters, at least one uppercase letter,
    /// one lowercase letter, one number, and one special character.
    /// </summary>
    public string Password { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the user's role in the system.
    /// Determines the user's permissions and access levels.
    /// </summary>
    public UserRole Role { get;     private set; }

    /// <summary>
    /// Gets the user's current status.
    /// Indicates whether the user is active, inactive, or blocked in the system.
    /// </summary>
    public UserStatus Status { get; private set; }

    /// <summary>
    /// Gets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time of the last update to the user's information.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    /// <returns>The user's ID as a string.</returns>
    string IUser.Id => Id.ToString();

    /// <summary>
    /// Gets the username.
    /// </summary>
    /// <returns>The username.</returns>
    string IUser.Username => Username;

    /// <summary>
    /// Gets the user's role in the system.
    /// </summary>
    /// <returns>The user's role as a string.</returns>
    string IUser.Role => Role.ToString();

    /// <summary>
    /// Initializes a new instance of the User class.
    /// </summary>
    public User()
    {
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the User class with all required properties.
    /// </summary>
    /// <param name="id">The user's identifier. Must be a valid GUID.</param>
    /// <param name="username">The user's username. Must be between 3 and 50 characters.</param>
    /// <param name="email">The user's email address. Must be in valid email format.</param>
    /// <param name="phone">The user's phone number. Must be in valid format.</param>
    /// <param name="password">The user's password. Must meet security requirements.</param>
    /// <param name="role">The user's role in the system.</param>
    /// <param name="status">The user's current status (defaults to Active).</param>
    /// <exception cref="ArgumentException">Thrown when any parameter fails validation.</exception>
    /// <exception cref="ArgumentNullException">Thrown when required parameters are null or empty.</exception>
    public User(
        Guid id,
        string username,
        string email,
        string phone,
        string password,
        UserRole role,
        UserStatus status = UserStatus.Active)
    {
        Id = id;
        SetUsername(username);
        SetEmail(email);
        SetPhone(phone);
        SetPassword(password);
        SetRole(role);
        SetStatus(status);
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the username with validation.
    /// </summary>
    /// <param name="username">The username to set.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when username is null, empty, or doesn't meet length requirements.
    /// </exception>
    public void SetUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username cannot be null or empty.");

        if (username.Length < 3 || username.Length > 50)
            throw new DomainException("Username must be between 3 and 50 characters.");

        if (!username.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '.' || c == '-'))
            throw new DomainException("Username can only contain letters, digits, underscores, dots, and hyphens.");

        Username = username.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the email address with validation.
    /// </summary>
    /// <param name="email">The email to set.</param>
    /// <exception cref="DomainException">
    /// Thrown when email is null, empty, or not in valid format.
    /// </exception>
    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be null or empty.");

        if (!IsValidEmail(email))
            throw new DomainException("Email is not in a valid format.");

        if (email.Length > 254)
            throw new DomainException("Email cannot exceed 254 characters.");

        Email = email.Trim().ToLowerInvariant();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the phone number with validation.
    /// </summary>
    /// <param name="phone">The phone number to set.</param>
    /// <exception cref="DomainException">
    /// Thrown when phone number is null, empty, or not in valid format.
    /// </exception>
    public void SetPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Phone cannot be null or empty.");

        var digitsOnly = new string([.. phone.Where(char.IsDigit)]);

        if (digitsOnly.Length < 10 || digitsOnly.Length > 15)
            throw new DomainException("Phone number must be between 10 and 15 digits.");

        Phone = digitsOnly;

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the password with security validation and hashing.
    /// </summary>
    /// <param name="password">The plain text password to set.</param>
    /// <exception cref="DomainException">
    /// Thrown when password doesn't meet security requirements.
    /// </exception>
    public void SetPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new DomainException("Password cannot be null or empty.");

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the user's role.
    /// </summary>
    /// <param name="role">The role to set.</param>
    public void SetRole(UserRole role)
    {
        if (!Enum.IsDefined(typeof(UserRole), role))
            throw new DomainException("Invalid user role.");

        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the user's status.
    /// </summary>
    /// <param name="status">The status to set.</param>
    public void SetStatus(UserStatus status)
    {
        if (!Enum.IsDefined(typeof(UserStatus), status))
            throw new DomainException("Invalid user status.");

        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates email format using basic regex.
    /// </summary>
    private static bool IsValidEmail(string email)
    {
        if (email.Length > 254)
            return false;

        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Activates the user account.
    /// Changes the user's status to Active.
    /// </summary>
    public void Activate()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the user account.
    /// Changes the user's status to Inactive.
    /// </summary>
    public void Deactivate()
    {
        Status = UserStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Blocks the user account.
    /// Changes the user's status to Blocked.
    /// </summary>
    public void Suspend()
    {
        Status = UserStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
    }
}