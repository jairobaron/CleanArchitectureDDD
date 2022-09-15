using System.Reflection;
using CleanArchitectureDDD.Application.Common.Exceptions;
using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Application.Common.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CleanArchitectureDDD.Application.Common.Behaviours;
/*
public abstract class ContextualRequest<TResponse>
    : IRequest<TResponse>
{
    public IDictionary<string, object> Items { get; }
        = new Dictionary<string, object>();
}

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ContextualRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehaviour(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        request.Items["CurrentUser"] = _httpContextAccessor.HttpContext?.User?.FindAll(x => x.Type == ClaimTypes.Sid);

        await next();
    }
}
*/