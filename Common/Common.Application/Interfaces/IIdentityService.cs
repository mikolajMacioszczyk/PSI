namespace Common.Application.Interfaces;

public interface IIdentityService
{
    public bool IsAuthenticated { get; }

    public Guid UserId { get; }

    public bool IsInRole(string roleName);
    public Guid? TryGetUserId();
}
