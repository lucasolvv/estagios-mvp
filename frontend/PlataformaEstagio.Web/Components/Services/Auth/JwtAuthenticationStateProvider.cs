using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _storage;
    private const string TokenKey = "auth.token";
    private readonly JwtSecurityTokenHandler _handler = new();

    private ClaimsPrincipal _user = new(new ClaimsIdentity());
    private bool _restored;

    public JwtAuthenticationStateProvider(ProtectedLocalStorage storage)
        => _storage = storage;

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(new AuthenticationState(_user));

    public async Task RestoreAsync()
    {
        SetAnonymous();

        var tokenEntry = await _storage.GetAsync<string>(TokenKey);
        var token = tokenEntry.Success ? tokenEntry.Value : null;
        if (string.IsNullOrWhiteSpace(token))
        {
            Notify();
            return;
        }

        try
        {
            var jwt = _handler.ReadJwtToken(token);

            // expiração básica (ValidTo já está em UTC)
            if (jwt.ValidTo <= DateTime.UtcNow)
            {
                await _storage.DeleteAsync(TokenKey); // limpa expirado
                Notify();
                return;
            }

            _user = new ClaimsPrincipal(CreateIdentityFromJwt(jwt));
            Notify();
        }
        catch
        {
            SetAnonymous();
            Notify();
        }
    }

    // ✅ Salva o token no storage ao logar
    public async Task NotifyUserAuthenticationAsync(string token)
    {
        await _storage.SetAsync(TokenKey, token);
        var jwt = _handler.ReadJwtToken(token);
        _user = new ClaimsPrincipal(CreateIdentityFromJwt(jwt));
        Notify();
    }

    // ✅ Remove do storage ao deslogar
    public async Task NotifyUserLogoutAsync()
    {
        await _storage.DeleteAsync(TokenKey);
        SetAnonymous();
        Notify();
    }

    private static ClaimsIdentity CreateIdentityFromJwt(JwtSecurityToken jwt)
    {
        string? Get(string type) => jwt.Claims.FirstOrDefault(c => c.Type == type)?.Value;

        var email = Get("email") ?? Get(ClaimTypes.Email);
        var role = Get("role") ?? Get(ClaimTypes.Role);
        var uid = Get("uid") ?? Get(ClaimTypes.NameIdentifier) ?? Get("sub");
        var name = Get("nickname") ?? Get(ClaimTypes.Name);
        var userTypeId = Get("userTypeId");

        var claims = new List<Claim>();
        if (!string.IsNullOrEmpty(uid)) claims.Add(new(ClaimTypes.NameIdentifier, uid));
        if (!string.IsNullOrEmpty(name)) claims.Add(new(ClaimTypes.Name, name));
        if (!string.IsNullOrEmpty(email)) claims.Add(new(ClaimTypes.Email, email));
        if (!string.IsNullOrEmpty(role)) claims.Add(new(ClaimTypes.Role, role));
        if (!string.IsNullOrEmpty(userTypeId)) claims.Add(new("userTypeId", userTypeId));

        // preserva demais claims
        foreach (var c in jwt.Claims)
            if (!claims.Any(k => k.Type == c.Type))
                claims.Add(c);

        return new ClaimsIdentity(claims, authenticationType: "jwt");
    }

    private void Notify()
        => NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));

    private void SetAnonymous()
        => _user = new ClaimsPrincipal(new ClaimsIdentity());
}
