using CleanArchitectureDDD.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureDDD.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly IUser _currentUserService;

    public LoggingBehaviour(ILogger<TRequest> logger, IUser currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.Id;

        //Request
        using(_logger.BeginScope("{ServiceName}", requestName))
        using (_logger.BeginScope("{CdUser}", userId))//Insert in additional column CdUser on Database
        {
            _logger.LogInformation("CleanArchitectureDDD Handling Request: {ServiceName} {@UserId} {@Request}",
                requestName, userId, request);

        }
    }
}
