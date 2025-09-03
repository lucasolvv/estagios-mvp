using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

public sealed class TokenHandler : DelegatingHandler
{
    private readonly IUserContext _user;

    public TokenHandler(IUserContext user) => _user = user;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        // Se o token existir, anexa
        var token = _user.AccessToken;
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return base.SendAsync(request, ct);
    }
}
