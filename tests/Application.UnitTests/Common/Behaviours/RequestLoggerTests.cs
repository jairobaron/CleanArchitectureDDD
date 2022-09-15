using CleanArchitectureDDD.Application.Common.Behaviours;
using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Application.Languages.Commands.CreateLanguage;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

namespace Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ILogger<CreateLanguageCommand>> _logger = null!;
    private Mock<ICurrentUserService> _currentUserService = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateLanguageCommand>>();
        _currentUserService = new Mock<ICurrentUserService>();
    }

    [Test]
    public async Task ShouldCallCurrentUserAsyncOnceIfAuthenticated()
    {
        var userId = _currentUserService.Setup(x => x.UserId).Returns(1);

        var requestLogger = new LoggingBehaviour<CreateLanguageCommand>(_logger.Object, _currentUserService.Object);

        await requestLogger.Process(new CreateLanguageCommand { DsLanguage = "English", DsPrefix = "ENG" }, new CancellationToken());

        userId.Equals(1);
    }
}
