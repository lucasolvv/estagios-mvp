using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PlataformaEstagio.Web.Components.Services.Auth
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _storage;
        private const string TokenKey = "auth.token";
        private readonly JwtSecurityTokenHandler _handler = new();

        private ClaimsPrincipal _user = new(new ClaimsIdentity());
        private bool _restored; // evita restaurar várias vezes em sequência


        public JwtAuthenticationStateProvider(ProtectedLocalStorage storage)
        {
            _storage = storage;
        }

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

                // expiração básica
                if (jwt.ValidTo.ToUniversalTime() <= DateTime.UtcNow)
                {
                    // opcional: limpar do storage
                    await _storage.DeleteAsync(TokenKey);
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

        public Task NotifyUserAuthenticationAsync(string token)
        {
            var jwt = _handler.ReadJwtToken(token);
            _user = new ClaimsPrincipal(CreateIdentityFromJwt(jwt));
            Notify();
            return Task.CompletedTask;
        }

        public Task NotifyUserLogoutAsync()
        {
            SetAnonymous();
            Notify();
            return Task.CompletedTask;
        }

        // 🔧 Normaliza nomes de claims (email/role podem vir mapeados)
        private static ClaimsIdentity CreateIdentityFromJwt(JwtSecurityToken jwt)
        {
            string? Get(string type) => jwt.Claims.FirstOrDefault(c => c.Type == type)?.Value;

            var email = Get("email") ?? Get(ClaimTypes.Email);
            var role = Get("role") ?? Get(ClaimTypes.Role);
            var uid = Get("uid") ?? Get(ClaimTypes.NameIdentifier) ?? Get("sub");
            var name = Get("nickname") ?? Get(ClaimTypes.Name);

            var claims = new List<Claim>();
            if (!string.IsNullOrEmpty(uid)) claims.Add(new(ClaimTypes.NameIdentifier, uid));
            if (!string.IsNullOrEmpty(name)) claims.Add(new(ClaimTypes.Name, name));
            if (!string.IsNullOrEmpty(email)) claims.Add(new(ClaimTypes.Email, email));
            if (!string.IsNullOrEmpty(role)) claims.Add(new(ClaimTypes.Role, role));

            // mantém claims adicionais (ex.: userTypeId)
            foreach (var c in jwt.Claims)
            {
                if (claims.Any(k => k.Type == c.Type)) continue; // evita duplicar os já normalizados
                claims.Add(c);
            }

            return new ClaimsIdentity(claims, authenticationType: "jwt");
        }

        private void Notify() =>
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));

        private void SetAnonymous() =>
            _user = new ClaimsPrincipal(new ClaimsIdentity());
    }
}
