namespace CleanArchitectureDDD.Application.FunctionalTests;

using System.Threading.Tasks;
using static Testing;

[TestFixture]
public abstract class BaseTestFixture
{
    [SetUp]
    public async Task TestSetUp()
    {
        await ResetState();
    }
}
