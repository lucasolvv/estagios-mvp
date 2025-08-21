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

        public async Task CreateUserAsync(RequestCreateUserJson user)
        {
            var resp = await _http.PostAsJsonAsync("api/user/new-user", user); // ajuste rota
            resp.EnsureSuccessStatusCode();
        }
    }

}
