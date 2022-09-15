using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using CleanArchitectureDDD.Application.Common.Models;
using CleanArchitectureDDD.Application.Languages.Queries.GetLanguages;
using CleanArchitectureDDD.Domain.Entities;

namespace CleanArchitectureDDD.Application.IntegrationTests.Languages.Queries;

using static Testing;
public class GetLanguagesWithPaginationTests : TestBase
{
    [Test]
    public async Task ShouldReturnAllLanguagesWithPagination()
    {
        //Arrange
        await AddAsync(new Language
        {
            DsLanguage = "English", DsPrefix = "ENG"
        });
        await AddAsync(new Language
        {
            DsLanguage = "Español", DsPrefix = "SPA"
        });
        var query = new GetLanguagesWithPaginationQuery(){ PageNumber = 1, PageSize = 10};

        //Act
        PaginatedList<LanguageBriefDto> result = await SendAsync(query);

        //Assert
        result.Should().NotBeNull();
        result.TotalCount.Should().Be(2);
        result.Items.Should().HaveCount(2);
    }
}
