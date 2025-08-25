using MudBlazor.Services;
using PlataformaEstagio.Web; // ajuste para o namespace onde está o componente App
using PlataformaEstagio.Web.Components;
using PlataformaEstagio.Web.Components.Services; // onde ficará o IUserServices/UserServices
using Microsoft.AspNetCore.Components.Routing;

var builder = WebApplication.CreateBuilder(args);

// Razor + Blazor interativo no servidor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// MudBlazor (Snackbar, Dialog, etc.)
builder.Services.AddMudServices();

// HttpClient tipado para sua API
builder.Services.AddHttpClient<IUserServices, UserServices>(client =>
{
    // Ajuste a URL base da API aqui ou via appsettings: "Backend:BaseUrl"
    client.BaseAddress = new Uri(
        builder.Configuration["Backend:BaseUrl"] ?? "https://localhost:7095/"
    );
});

builder.Services.AddProtectedBrowserStorage();

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
