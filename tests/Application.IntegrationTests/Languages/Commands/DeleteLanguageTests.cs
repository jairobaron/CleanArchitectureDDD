using System;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitectureDDD.Application.Common.Exceptions;
using CleanArchitectureDDD.Application.Languages.Commands.CreateLanguage;
using CleanArchitectureDDD.Application.Languages.Commands.UpdateLanguage;
using CleanArchitectureDDD.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using CleanArchitectureDDD.Application.Languages.Commands.DeleteLanguage;

namespace CleanArchitectureDDD.Application.IntegrationTests.Languages.Commands;

using static Testing;

public class DeleteLanguageTests : TestBase
{
    [Test]
    public async Task ShouldRequireValidLanguageId()
    {
        var command = new DeleteLanguageCommand { CdLanguage = 9999 };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteLanguage()
    {
        var userId = await RunAsDefaultUserAsync();

        var languageId = await SendAsync(new CreateLanguageCommand
        {
            DsLanguage = "English",
            DsPrefix =  "ENG"
        });

        var command = new DeleteLanguageCommand
        {
            CdLanguage = languageId
        };

        await SendAsync(command);

        var language = await FindAsync<Language>(languageId);

        language.Should().NotBeNull();
        language!.IsLogicalDelete.Should().Be(1);
        language.CdUserUpdateAud.Should().NotBeNull();
        language.CdUserUpdateAud.Should().Be(userId);
        language.DtUserUpdateAud.Should().NotBeNull();
        language.DtUserUpdateAud.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
