using System.Net.Http.Json;
using PlataformaEstagios.Communication.Requests;

namespace PlataformaEstagio.Web.Components.Services
{
    public interface IUserServices
    {
        Task CreateUserAsync(RequestCreateUserJson user);
    }

    public class UserServices : IUserServices
    {
        private readonly HttpClient _http;
        public UserServices(HttpClient http) => _http = http;
        public class ApiError
        {
            public string? Code { get; set; }
            public string? Message { get; set; }
            public List<string>? Errors { get; set; }
        }

        public async Task CreateUserAsync(RequestCreateUserJson user)
        {
            var resp = await _http.PostAsJsonAsync("api/user/new-user", user); // rota da sua API
            if (resp.IsSuccessStatusCode) return;

            ApiError? apiErr = null;
            try
            {
                apiErr = await resp.Content.ReadFromJsonAsync<ApiError>();
            }
            catch { /* se vier HTML/sem JSON, cai no fallback abaixo */ }

            var msgs = (apiErr?.Errors != null && apiErr.Errors.Count > 0)
                ? apiErr.Errors
                : new List<string> { apiErr?.Message ?? $"Erro HTTP {(int)resp.StatusCode}" };

            // Dispare uma exceção amigável para o componente tratar
            throw new ApplicationException(string.Join("\n", msgs));
        }
    }

}
