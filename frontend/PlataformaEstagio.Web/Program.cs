using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using PlataformaEstagio.Web.Components;
using PlataformaEstagio.Web.Components.Services;
using PlataformaEstagio.Web.Components.Services.Auth;
using PlataformaEstagios.Web.Services.Auth; // onde ficará o IUserServices/UserServices


var builder = WebApplication.CreateBuilder(args);

// Razor + Blazor interativo no servidor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// MudBlazor (Snackbar, Dialog, etc.)
builder.Services.AddMudServices();




builder.Services.AddHttpClient("Backend", (sp, client) =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var baseUrl = cfg["Backend:BaseUrl"] ?? "https://localhost:7095/";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<HttpClient>(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("Backend"));

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpClient<IUserServices, UserServices>("Backend");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseStatusCodePagesWithReExecute("/not-found"); // middleware de reexecução para 404
app.UseAntiforgery();
app.MapRazorComponents<App>()          // seu componente raiz
   .AddInteractiveServerRenderMode();   // interativo no servidor

app.Run();
