using System.Diagnostics;
using CleanArchitectureDDD.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace CleanArchitectureDDD.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly IMetricReporterService _metricReporterService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PerformanceBehaviour(
        IMetricReporterService metricReporterService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _timer = new Stopwatch();
        _metricReporterService = metricReporterService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        CancellationToken cancellationToken, 
        RequestHandlerDelegate<TResponse> next)
    {
        var path = _httpContextAccessor.HttpContext?.Request.Path.Value;
        if (path == "/metrics" || path == null)
        {
            return await next();
        }

        _timer.Start();

        var response = await next();

        _timer.Stop();

        _metricReporterService.RegisterRequest();
        _metricReporterService.RegisterResponseTime(
            _httpContextAccessor.HttpContext!.Response.StatusCode, 
            path,
            _httpContextAccessor.HttpContext.Request.Method, 
            _timer.Elapsed);

        return response;
    }
}
