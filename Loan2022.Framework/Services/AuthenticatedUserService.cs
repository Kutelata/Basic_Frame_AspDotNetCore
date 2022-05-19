using System.Security.Claims;
using Loan2022.Application.Interfaces.Shared;

namespace Loan2022.Framework.Services;

public class AuthenticatedUserService: IAuthenticatedUserService
{
    private readonly IHttpContextAccessor _context;
    public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
    {
        _context = httpContextAccessor;
    }

    public string? UserId => _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

    public string Username => _context.HttpContext?.User?.FindFirst(ClaimTypes.Name) == null ? null : _context.HttpContext?.User?.FindFirst(ClaimTypes.Name).Value;
}