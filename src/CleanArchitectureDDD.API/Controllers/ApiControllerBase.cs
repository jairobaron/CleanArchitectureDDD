using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDDD.API.Controllers;
/// <summary>
/// Api Controller Base 
/// </summary>
[ApiController]
[Route("api/v1/config/[controller]/[action]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator = null!;
    /// <summary>
    /// Mediator Pattern
    /// </summary>
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
