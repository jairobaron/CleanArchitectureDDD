using CleanArchitectureDDD.Domain.Exceptions;
using CleanArchitectureDDD.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.UnitTests.ValueObjects;

public class PrefixTests
{
    [Test]
    public void ShouldReturnCorrectPrefixCode()
    {
        var code = "ENG";

        var colour = Prefix.From(code);

        colour.Code.Should().Be(code);
    }

    [Test]
    public void ToStringReturnsCode()
    {
        var prefix = Prefix.English;

        prefix.ToString().Should().Be(prefix.Code);
    }

    [Test]
    public void ShouldPerformImplicitConversionToPrefixCodeString()
    {
        string code = Prefix.English;

        code.Should().Be("ENG");
    }

    [Test]
    public void ShouldPerformExplicitConversionGivenSupportedPrefixCode()
    {
        var prefix = (Prefix)"ENG";

        prefix.Should().Be(Prefix.English);
    }

    [Test]
    public void ShouldThrowUnsupportedColourExceptionGivenNotSupportedPrefixCode()
    {
        FluentActions.Invoking(() => Prefix.From("NLD"))
            .Should().Throw<UnsupportedLanguageException>();
    }
}
