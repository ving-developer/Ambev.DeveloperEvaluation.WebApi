using System;

namespace Ambev.DeveloperEvaluation.Domain.Exceptions;

/// <summary>
/// Thrown when a requested entity cannot be found.
/// </summary>
public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityName, object key)
        : base($"{entityName} with key '{key}' was not found.")
    { }
}
