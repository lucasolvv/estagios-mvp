using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

public interface IUserContext
{
    bool IsAuthenticated { get; }
    string? Email { get; }
    string? Nickname { get; }
    string? Role { get; } // "Enterprise" | "Candidate"
    Guid? UserId { get; }
    Guid? UserTypeId { get; }

    // NOVO: token atual para anexar no Authorization
    string? AccessToken { get; }

    Task RefreshAsync(); // chama em OnInitializedAsync()

    void SetToken(string? token); // chama após login/logout
}

public sealed class UserContext : IUserContext
{
    private readonly AuthenticationStateProvider _provider;
    private readonly ProtectedLocalStorage _storage;

    private ClaimsPrincipal _user = new(new ClaimsIdentity());
    private const string TokenKey = "auth.token";

    public UserContext(AuthenticationStateProvider provider, ProtectedLocalStorage storage)
    {
        _provider = provider;
        _storage = storage;
        
        _provider.AuthenticationStateChanged += async task =>
        {
            var state = await task;
            _user = state.User;                 // atualiza claims assim que o provider notificar
                                                // não mexa no storage aqui; token em memória já foi setado por AuthService/AuthInitializer
        };
    }

    public bool IsAuthenticated => _user.Identity?.IsAuthenticated ?? false;
    public string? Email => _user.FindFirst(ClaimTypes.Email)?.Value;
    public string? Nickname => _user.FindFirst("nickname")?.Value ?? _user.Identity?.Name;
    public string? Role => _user.FindFirst(ClaimTypes.Role)?.Value ?? _user.FindFirst("role")?.Value;
    public Guid? UserId => TryGuid(_user.FindFirst("sub")?.Value);
    public Guid? UserTypeId => TryGuid(_user.FindFirst("userTypeId")?.Value);

    public string? AccessToken { get; private set; }

    public void SetToken(string? token) => AccessToken = token;

    public async Task RefreshAsync()
    {
        // 1) Claims do provedor
        var state = await _provider.GetAuthenticationStateAsync();
        _user = state.User;

        // 2) (OK continuar lendo do storage AQUI, mas só chame isso após o primeiro render)
        var stored = await _storage.GetAsync<string>(TokenKey);
        AccessToken = stored.Success ? stored.Value : null;

    }

    private static Guid? TryGuid(string? s) => Guid.TryParse(s, out var g) ? g : null;
}
