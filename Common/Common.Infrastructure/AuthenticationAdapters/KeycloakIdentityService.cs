using Common.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Common.Infrastructure.AuthenticationAdapters;

public class KeycloakIdentityService : IIdentityService
{
    private readonly HttpContext _httpContext;

    public bool IsAuthenticated
        => _httpContext.User.Identity?.IsAuthenticated ?? false;

    public Guid UserId
        => Guid.TryParse(_httpContext.User.FindFirst(KeycloakClaims.IdClaimType)?.Value, out Guid id) ? id : Guid.Empty;

    public KeycloakIdentityService(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext);

        _httpContext = httpContextAccessor.HttpContext;
    }

    public bool IsInRole(string roleName) => _httpContext.User.IsInRole(roleName);

    public Guid? TryGetUserId() =>
        Guid.TryParse(_httpContext.User.FindFirst(KeycloakClaims.IdClaimType)?.Value, out Guid id) ? id : null;
}
