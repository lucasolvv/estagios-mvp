using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using PlataformaEstagios.Domain.Entities;
using System.Globalization;
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
        public JwtAuthenticationStateProvider(ProtectedLocalStorage storage)
        {
            _storage = storage;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(new AuthenticationState(_user));
        public async Task RestoreAsync()
        {
            SetAnonymous();

            var tokenEntry = await _storage.GetAsync<string>(TokenKey); // JS interop OK após primeiro render
            var token = tokenEntry.Success ? tokenEntry.Value : null;
            if (string.IsNullOrWhiteSpace(token)) { Notify(); return; }
            
            try
            {
                var jwt = _handler.ReadJwtToken(token);
                // validação básica de expiração
                if (jwt.ValidTo.ToUniversalTime() <= DateTime.UtcNow)
                {
                    // expirou: mantém anônimo e limpa storage se quiser
                    Notify();
                    return;
                }
                _user = new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims, "jwt"));
                Notify();
            }
            catch
            {
                // token inválido: fica anônimo
                SetAnonymous();
                Notify();
            }

        }
        public Task NotifyUserAuthenticationAsync(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            _user = new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims, "jwt"));
            Notify();
            return Task.CompletedTask;
        }

        public Task NotifyUserLogoutAsync()
        {
            SetAnonymous();
            Notify();
            return Task.CompletedTask;
        }

        private static ClaimsIdentity CreateIdentityFromJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            // Claims típicos emitidos no backend:
            // sub (UserIdentifier), nickname, email, role (UserType)
            var claims = new List<Claim>(jwt.Claims);

            return new ClaimsIdentity(claims, authenticationType: "jwt");
        }

        private void Notify() =>
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));

        private void SetAnonymous() =>
        _user = new ClaimsPrincipal(new ClaimsIdentity());

    }
}
