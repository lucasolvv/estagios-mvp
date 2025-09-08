using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor.Services;
using PlataformaEstagio.Web.Components;
using PlataformaEstagio.Web.Components.Services;
using PlataformaEstagio.Web.Components.Services.Auth;
using PlataformaEstagio.Web.Components.Services.Candidate;
using PlataformaEstagio.Web.Components.Services.Enterprise;
using PlataformaEstagios.Web.Services.Auth;


var builder = WebApplication.CreateBuilder(args);

// Razor + Blazor interativo no servidor
builder.Services.AddRazorComponents()
     .AddInteractiveServerComponents(options => options.DetailedErrors = true);

// MudBlazor (Snackbar, Dialog, etc.)
builder.Services.AddMudServices();


// DI
builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddHttpClient("Backend", (sp, c) =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    c.BaseAddress = new Uri(cfg["Backend:BaseUrl"] ?? "https://localhost:7095/");
});


builder.Services.AddHttpClient("BackendRaw", (sp, c) =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    c.BaseAddress = new Uri(cfg["Backend:BaseUrl"] ?? "https://localhost:7095/");
});

builder.Services.AddAuthorizationCore(o =>
{
    o.AddPolicy("IsEnterprise", p => p.RequireRole("Enterprise"));
    o.AddPolicy("IsCandidate", p => p.RequireRole("Candidate"));
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();




builder.Services.AddHttpClient<IAuthService, AuthService>("BackendRaw");
builder.Services.AddHttpClient<IUserServices, UserServices>("Backend");
builder.Services.AddHttpClient<IEnterpriseService, EnterpriseService>("Backend");
builder.Services.AddHttpClient<ICandidateService, CandidateService>("Backend");


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>()  
   .AddInteractiveServerRenderMode();

app.Run();
