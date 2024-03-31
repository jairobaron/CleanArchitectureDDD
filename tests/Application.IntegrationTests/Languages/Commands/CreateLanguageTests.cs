using CleanArchitectureDDD.Application.Common.Exceptions;
using CleanArchitectureDDD.Application.Languages.Commands.CreateLanguage;
using CleanArchitectureDDD.Domain.Entities;

namespace CleanArchitectureDDD.Application.FunctionalTests.Languages.Commands;

using static Testing;

public class CreateLanguageTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateLanguageCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }
    
    [Test]
    public async Task ShouldRequireUniqueLanguage()
    {
        await SendAsync(new CreateLanguageCommand
        {
            DsLanguage = "English",
            DsPrefix = "ENG"
        });

        var command = new CreateLanguageCommand
        {
            DsLanguage = "English",
            DsPrefix = "ENG"
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateLanguage()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateLanguageCommand
        {
            DsLanguage = "English",
            DsPrefix = "ENG"
        };

        var id = await SendAsync(command);

        var language = await FindAsync<Language>(id);

        language.Should().NotBeNull();
        language!.DsLanguage.Should().Be(command.DsLanguage);
        language.IdCreatedAud.Should().Be(userId);
        language.DtCreatedAud.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        language.IdUpdatedAud.Should().Be(userId);
        language.DtUpdatedAud.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
