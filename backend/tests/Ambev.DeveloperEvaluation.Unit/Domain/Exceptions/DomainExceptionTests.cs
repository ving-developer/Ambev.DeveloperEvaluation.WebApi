using Ambev.DeveloperEvaluation.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Exceptions;

public class DomainExceptionTests
{
    [Fact(DisplayName = "Given message, when creating DomainException, then message is set")]
    public void Given_Message_When_CreatingDomainException_Then_MessageIsSet()
    {
        // Given
        var message = "Test domain exception";

        // When
        var exception = new DomainException(message);

        // Then
        exception.Message.Should().Be(message);
        exception.InnerException.Should().BeNull();
    }

    [Fact(DisplayName = "Given message and inner exception, when creating DomainException, then properties are set")]
    public void Given_MessageAndInnerException_When_CreatingDomainException_Then_PropertiesAreSet()
    {
        // Given
        var message = "Test domain exception";
        var inner = new InvalidOperationException("Inner exception");

        // When
        var exception = new DomainException(message, inner);

        // Then
        exception.Message.Should().Be(message);
        exception.InnerException.Should().Be(inner);
    }
}
