using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor.Services;
using PlataformaEstagio.Web.Components;
using PlataformaEstagio.Web.Components.Services;
using PlataformaEstagio.Web.Components.Services.Auth;
using PlataformaEstagio.Web.Components.Services.Candidate;
using PlataformaEstagio.Web.Components.Services.Enterprise;
using PlataformaEstagios.Web.Services.Auth; // onde ficará o IUserServices/UserServices


var builder = WebApplication.CreateBuilder(args);

// Razor + Blazor interativo no servidor
builder.Services.AddRazorComponents()
     .AddInteractiveServerComponents(options => options.DetailedErrors = true);
    //.AddInteractiveServerComponents();

// MudBlazor (Snackbar, Dialog, etc.)
builder.Services.AddMudServices();


// DI
builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddHttpClient("Backend", (sp, c) =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    c.BaseAddress = new Uri(cfg["Backend:BaseUrl"] ?? "https://localhost:7095/");
});
//.AddHttpMessageHandler(sp => new TokenHandler(sp.GetRequiredService<IUserContext>()));


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

//builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Backend"));

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
//app.UseStatusCodePagesWithReExecute("/not-found"); // middleware de reexecução para 404
app.UseAntiforgery();
app.MapRazorComponents<App>()          // seu componente raiz
   .AddInteractiveServerRenderMode();   // interativo no servidor

app.Run();
