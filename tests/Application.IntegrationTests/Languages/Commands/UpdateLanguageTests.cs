using System.Linq;
using CleanArchitectureDDD.Application.Common.Exceptions;
using CleanArchitectureDDD.Application.Languages.Commands.CreateLanguage;
using CleanArchitectureDDD.Application.Languages.Commands.UpdateLanguage;
using CleanArchitectureDDD.Domain.Entities;

namespace CleanArchitectureDDD.Application.FunctionalTests.Languages.Commands;

using static Testing;
using NotFoundException = Common.Exceptions.NotFoundException;

public class UpdateLanguageTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidLanguageId()
    {
        var command = new UpdateLanguageCommand { Id = Guid.NewGuid(), DsLanguage = "English", DsPrefix = "ENG" };
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
            Id = languageId,
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
            Id = languageId,
            DsLanguage = "Chinese",
            DsPrefix = "CHN"
        };

        await SendAsync(command);

        var language = await FindAsync<Language>(languageId);

        language.Should().NotBeNull();
        language!.DsLanguage.Should().Be(command.DsLanguage);
        language.IdUpdatedAud.Should().NotBeNull();
        language.IdUpdatedAud.Should().Be(userId);
        language.DtUpdatedAud.Should().NotBeNull();
        language.DtUpdatedAud.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
