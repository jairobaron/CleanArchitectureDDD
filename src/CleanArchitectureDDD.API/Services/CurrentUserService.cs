using System.Security.Claims;
using CleanArchitectureDDD.Application.Common.Interfaces;

namespace CleanArchitectureDDD.API.Services;
/// <summary>
/// Class Get Current User
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    /// <summary>
    /// Class Constructor
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    /// <summary>
    /// Get User Id of current user
    /// </summary>
    //public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public long UserId => long.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Sid)??"1");
}
