using IntranetGCM.Components;
using IntranetGCM.Data;
using IntranetGCM.Models;
using IntranetGCM.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

#region 🔧 CONFIGURAÇÃO DO SERVIDOR (KESTREL)
// Define que a aplicação vai escutar em qualquer IP na porta 5010 (HTTP)
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5010);
});
#endregion

#region 🗄️ BANCO DE DADOS (EF CORE)
// Configura o DbContext com SQL Server usando connection string do appsettings
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region 🔐 IDENTITY (USUÁRIOS, ROLES, TOKENS)
// Configura o sistema de identidade (login, senha, roles, etc.)
builder.Services.AddIdentity<Usuario, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
#endregion

#region 🍪 COOKIE DO IDENTITY (CONFIGURAÇÃO PRINCIPAL)
// Configura o cookie usado pelo Identity (login)
builder.Services.ConfigureApplicationCookie(options =>
{
    // Rotas
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/access-denied";

    // Cookie
    options.Cookie.Name = "auth_token";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // DEV (em prod: Always)

    // Expiração
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true; // renova a cada requisição

    // Comportamento de redirect (importante pra API/Blazor)
    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/login"))
            return Task.CompletedTask;

        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = 403;
        return Task.CompletedTask;
    };
});
#endregion

#region 🛡️ AUTORIZAÇÃO
// Habilita uso de [Authorize]
builder.Services.AddAuthorization();

// Necessário para Blazor reconhecer o estado de autenticação
builder.Services.AddCascadingAuthenticationState();
#endregion

#region ⚛️ BLAZOR SERVER
// Configuração do Blazor Server (componentes interativos)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
#endregion

#region 📦 SERVIÇOS DA APLICAÇÃO
// Injeção de dependência dos serviços internos
builder.Services.AddScoped<NoticiaService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddBlazorBootstrap();
builder.Services.AddHttpContextAccessor();
#endregion

#region 🌍 HTTP CLIENT (CHAMADAS INTERNAS)
// Cliente HTTP global para chamadas à própria API
builder.Services.AddHttpClient("server", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5010");
});

// Injeta o client padrão
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("server"));
#endregion

#region 👤 SEED DE ROLES
// Cria roles padrão no banco ao iniciar a aplicação
async Task SeedRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
#endregion

var app = builder.Build();

#region 🌱 EXECUÇÃO DE SEED (INICIALIZAÇÃO)
using (var scope = app.Services.CreateScope())
{
    await SeedRoles(scope.ServiceProvider);
}
#endregion

#region ⚙️ MIDDLEWARES (PIPELINE HTTP)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// Página para status code (404, etc.)
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// Redireciona HTTP → HTTPS (cuidado: você está rodando HTTP puro)
app.UseHttpsRedirection();

// Autenticação e autorização (ordem IMPORTANTE)
app.UseAuthentication();
app.UseAuthorization();

// Proteção contra CSRF
app.UseAntiforgery();
#endregion

#region 📁 STATIC + BLAZOR
// Arquivos estáticos (css, js, etc.)
app.MapStaticAssets();

// Mapeamento dos componentes Blazor
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
#endregion

#region 🔑 ENDPOINTS CUSTOM (LOGIN, ETC)
// Seus endpoints de autenticação
app.MapAuthEndpoints();
#endregion

#region 🚀 START
app.Run();
#endregion