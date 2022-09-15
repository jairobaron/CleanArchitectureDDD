using System;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitectureDDD.Application.Common.Exceptions;
using CleanArchitectureDDD.Application.Languages.Commands.CreateLanguage;
using CleanArchitectureDDD.Application.Languages.Commands.UpdateLanguage;
using CleanArchitectureDDD.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace CleanArchitectureDDD.Application.IntegrationTests.Languages.Commands;

using static Testing;

public class UpdateLanguageTests : TestBase
{
    [Test]
    public async Task ShouldRequireValidLanguageId()
    {
        var command = new UpdateLanguageCommand { CdLanguage = 9999, DsLanguage = "English", DsPrefix = "ENG" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireUniqueLanguage()
    {
        var languageId = await SendAsync(new CreateLanguageCommand
        {
            DsLanguage = "English",
            DsPrefix = "ENG"
        });

        await SendAsync(new CreateLanguageCommand
        {
            DsLanguage = "Spanish",
            DsPrefix = "ESP"
        });

        var command = new UpdateLanguageCommand
        {
            CdLanguage = languageId,
            DsLanguage = "Spanish",
            DsPrefix = "ESP"
        };

        (await FluentActions.Invoking(() =>
            SendAsync(command))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("DsLanguage")))
                .And.Errors["DsLanguage"].Should().Contain("The specified description already exists.");
    }

    [Test]
    public async Task ShouldUpdateLanguage()
    {
        var userId = await RunAsDefaultUserAsync();

        var languageId = await SendAsync(new CreateLanguageCommand
        {
            DsLanguage = "English",
            DsPrefix =  "ENG"
        });

        var command = new UpdateLanguageCommand
        {
            CdLanguage = languageId,
            DsLanguage = "Chinese",
            DsPrefix = "CHN"
        };

        await SendAsync(command);

        var language = await FindAsync<Language>(languageId);

        language.Should().NotBeNull();
        language!.DsLanguage.Should().Be(command.DsLanguage);
        language.CdUserUpdateAud.Should().NotBeNull();
        language.CdUserUpdateAud.Should().Be(userId);
        language.DtUserUpdateAud.Should().NotBeNull();
        language.DtUserUpdateAud.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
