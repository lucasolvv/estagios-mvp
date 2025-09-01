using Microsoft.AspNetCore.Components.Authorization;
using PlataformaEstagio.Web.Components.Helpers;
using System.Security.Claims;

public interface IUserContext
{
    bool IsAuthenticated { get; }
    string? Email { get; }
    string? Nickname { get; }
    string? Role { get; } // "Enterprise" | "Candidate"
    Guid? UserId { get; }
    Guid? UserTypeId { get; }

    Task RefreshAsync(); // chama em OnInitializedAsync()
}

public sealed class UserContext : IUserContext
{
    private readonly AuthenticationStateProvider _provider;
    private ClaimsPrincipal _user = new(new ClaimsIdentity());

    public UserContext(AuthenticationStateProvider provider) => _provider = provider;

    public bool IsAuthenticated => _user.Identity?.IsAuthenticated ?? false;
    public string? Email => _user.GetEmail();
    public string? Nickname => _user.GetNickname();
    public string? Role => _user.GetRole();
    public Guid? UserId => _user.GetUserId();
    public Guid? UserTypeId => _user.GetUserTypeId();

    public async Task RefreshAsync()
    {
        var state = await _provider.GetAuthenticationStateAsync();
        _user = state.User;
    }
}
