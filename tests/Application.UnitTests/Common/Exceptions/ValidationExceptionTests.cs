using CleanArchitectureDDD.Application.Common.Exceptions;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CleanArchitectureDDD.Application.UnitTests.Common.Exceptions;

public class ValidationExceptionTests
{
    [Test]
    public void DefaultConstructorCreatesAnEmptyErrorDictionary()
    {
        var actual = new ValidationException().Errors;

        actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
    }

    [Test]
    public void SingleValidationFailureCreatesASingleElementErrorDictionary()
    {
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("DsLanguage", "Maximum Length must be 200 or less"),
            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.Should().BeEquivalentTo(new string[] { "DsLanguage" });
        actual["DsLanguage"].Should().BeEquivalentTo(new string[] { "Maximum Length must be 200 or less" });
    }

    [Test]
    public void MulitpleValidationFailureForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
    {
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("DsLanguage", "Maximum Length must be 200 or less"),
                new ValidationFailure("DsLanguage", "Maximum Length must be 200"),
                new ValidationFailure("DsPrefix", "Maximum Length must be 10 or less"),
                new ValidationFailure("DsPrefix", "Maximum Length must be 10"),
            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.Should().BeEquivalentTo(new string[] { "DsLanguage", "DsPrefix" });

        actual["DsLanguage"].Should().BeEquivalentTo(new string[]
        {
                "Maximum Length must be 200 or less",
                "Maximum Length must be 200",
        });

        actual["DsPrefix"].Should().BeEquivalentTo(new string[]
        {
                "Maximum Length must be 10 or less",
                "Maximum Length must be 10",
        });
    }
}
