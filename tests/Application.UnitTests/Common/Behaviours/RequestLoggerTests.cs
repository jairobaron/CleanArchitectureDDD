using CleanArchitectureDDD.Application.Common.Behaviours;
using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Application.Languages.Commands.CreateLanguage;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using System;

namespace Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ILogger<CreateLanguageCommand>> _logger = null!;
    private Mock<IUser> _currentUserService = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateLanguageCommand>>();
        _currentUserService = new Mock<IUser>();
    }

    [Test]
    public async Task ShouldCallCurrentUserAsyncOnceIfAuthenticated()
    {
        var idGuid  = Guid.NewGuid;
        var userId = _currentUserService.Setup(x => x.Id).Returns(idGuid);

        var requestLogger = new LoggingBehaviour<CreateLanguageCommand>(_logger.Object, _currentUserService.Object);

        await requestLogger.Process(new CreateLanguageCommand { DsLanguage = "English", DsPrefix = "ENG" }, new CancellationToken());

        userId.Equals(idGuid);
    }
}
