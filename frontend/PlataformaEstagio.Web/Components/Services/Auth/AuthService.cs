using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using PlataformaEstagio.Web.Components.Services.Auth;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
namespace PlataformaEstagios.Web.Services.Auth
{
    public sealed class AuthService : IAuthService
    {
        private const string TokenKey = "auth.token";
        private const string SessionKey = "auth.session"; // guarda LoginResponseJson
        private readonly ProtectedLocalStorage _storage;
        private readonly AuthenticationStateProvider _authProvider;
        private readonly NavigationManager _nav;
        private readonly IUserContext _userContext;

        // cache em memória para o circuito atual (mais rápido que ir no storage toda hora)
        private string? _tokenCache;
        private ResponseLoginJson? _sessionCache;

        private readonly HttpClient _http; // não use handler global; use EnsureAuthHeaderAsync por chamada

        public AuthService(
            ProtectedLocalStorage storage,
            AuthenticationStateProvider authProvider,
            NavigationManager nav,
            HttpClient http,
            IUserContext userContext)
        {
            _storage = storage;
            _authProvider = authProvider;
            _nav = nav;
            _http = http;
            _userContext = userContext;
        }

        public async Task<bool> LoginAsync(RequestLoginJson request)
        {
            var resp = await _http.PostAsJsonAsync("api/auth/login", request);
            if (!resp.IsSuccessStatusCode) return false;

            var login = await resp.Content.ReadFromJsonAsync<ResponseLoginJson>();
            if (login is null || string.IsNullOrWhiteSpace(login.AccessToken)) return false;

            _tokenCache = login.AccessToken;
            _sessionCache = login;

            await _storage.SetAsync(TokenKey, login.AccessToken);
            await _storage.SetAsync(SessionKey, login);

            // atualiza memória para o handler
            _userContext.SetToken(login.AccessToken); // <<

            if (_authProvider is JwtAuthenticationStateProvider jwtProvider)
                await jwtProvider.NotifyUserAuthenticationAsync(login.AccessToken);

            return true;
        }

        public async Task LogoutAsync()
        {
            _tokenCache = null;
            _sessionCache = null;
            await _storage.DeleteAsync(TokenKey);
            await _storage.DeleteAsync(SessionKey);
            _userContext.SetToken(null); // <<
            if (_authProvider is JwtAuthenticationStateProvider jwtProvider)
                await jwtProvider.NotifyUserLogoutAsync();

            _nav.NavigateTo("/", forceLoad: true);
        }

        public async Task<string?> GetTokenAsync()
        {
            if (!string.IsNullOrWhiteSpace(_tokenCache)) return _tokenCache;

            var result = await _storage.GetAsync<string>(TokenKey);
            _tokenCache = result.Success ? result.Value : null;
            return _tokenCache;
        }

        public async Task<ResponseLoginJson?> GetSessionAsync()
        {
            if (_sessionCache is not null) return _sessionCache;

            var result = await _storage.GetAsync<ResponseLoginJson>(SessionKey);
            _sessionCache = result.Success ? result.Value : null;
            return _sessionCache;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrWhiteSpace(token)) return false;

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwt.ValidTo.ToUniversalTime() > DateTime.UtcNow;
        }

        // 👉 use isso antes de cada chamada protegida para anexar o Bearer
        public async Task EnsureAuthHeaderAsync(HttpClient http)
        {
            var token = await GetTokenAsync();
            if (!string.IsNullOrWhiteSpace(token))
                http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            else
                http.DefaultRequestHeaders.Authorization = null;
        }
    }
}
