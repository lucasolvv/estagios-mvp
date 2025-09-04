using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;

public abstract class BaseApiService
{
    private readonly HttpClient _http;
    private readonly IUserContext _user;

    protected BaseApiService(HttpClient http, IUserContext user)
    {
        _http = http;
        _user = user;
    }

    // --- Helpers de envio (sempre com Authorization, se houver token)
    protected async Task<T?> GetJsonAsync<T>(string uri, CancellationToken ct = default)
    {
        using var req = new HttpRequestMessage(HttpMethod.Get, uri);
        AttachBearer(req);
        using var resp = await _http.SendAsync(req, ct);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<T>(cancellationToken: ct);
    }

    protected async Task<TRes?> PostJsonAsync<TReq, TRes>(string uri, TReq body, CancellationToken ct = default)
    {
        using var req = new HttpRequestMessage(HttpMethod.Post, uri)
        { Content = JsonContent.Create(body) };
        AttachBearer(req);
        using var resp = await _http.SendAsync(req, ct);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<TRes>(cancellationToken: ct);
    }

    protected async Task DeleteAsync(string uri, CancellationToken ct = default)
    {
        using var req = new HttpRequestMessage(HttpMethod.Delete, uri);
        AttachBearer(req);
        using var resp = await _http.SendAsync(req, ct);
        resp.EnsureSuccessStatusCode();
    }
    protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage req, CancellationToken ct = default)
    {
        AttachBearer(req);
        return await _http.SendAsync(req, ct);
    }

    protected async Task<T?> SendJsonAsync<T>(HttpRequestMessage req, CancellationToken ct = default)
    {
        AttachBearer(req);
        var resp = await _http.SendAsync(req, ct);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<T>(cancellationToken: ct);
    }

    // Anexa o token do circuito atual (em memória)
    private void AttachBearer(HttpRequestMessage req)
    {
        var token = _user.AccessToken;
        if (!string.IsNullOrWhiteSpace(token))
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        else
            req.Headers.Authorization = null; // vai sem auth
    }
}
