using CleanArchitectureDDD.Application.Languages.Commands.CreateLanguage;
using CleanArchitectureDDD.Domain.Entities;
using CleanArchitectureDDD.Application.Languages.Commands.DeleteLanguage;

namespace CleanArchitectureDDD.Application.FunctionalTests.Languages.Commands;

using static Testing;
using NotFoundException = Common.Exceptions.NotFoundException;

public class DeleteLanguageTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidLanguageId()
    {
        var command = new DeleteLanguageCommand(Guid.NewGuid());
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

        var command = new DeleteLanguageCommand(languageId);

        await SendAsync(command);

        var language = await FindAsync<Language>(languageId);

        language.Should().NotBeNull();
        language!.IsLogicalDelete.Should().Be(1);
        language.IdDeletedAud.Should().NotBeNull();
        language.IdDeletedAud.Should().Be(userId);
        language.DtDeletedAud.Should().NotBeNull();
        language.DtDeletedAud.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
