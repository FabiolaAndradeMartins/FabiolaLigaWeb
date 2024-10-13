using LigaWeb.Data;
using LigaWeb.Data.Entities;
using LigaWeb.Data.Repositories.Impl;
using LigaWeb.Data.Repositories.Interfaces;
using LigaWeb.Helpers;
using LigaWeb.Helpers.Impl;
using LigaWeb.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(cfg =>
{
    cfg.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<User, IdentityRole>(cfg =>
{
    cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    cfg.SignIn.RequireConfirmedEmail = true;
    cfg.User.RequireUniqueEmail = true;
    cfg.Password.RequireDigit = false;
    cfg.Password.RequiredUniqueChars = 0;
    cfg.Password.RequireUppercase = false;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequiredLength = 6;
})
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<DataContext>();

var jwtSettings = builder.Configuration.GetSection("Tokens").Get<JwtSettings>();

builder.Services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });

// Registrar serviços personalizados no contêiner de DI
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IStadiumRepository, StadiumRepository>();
builder.Services.AddScoped<IClubRepository, ClubRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();

builder.Services.AddScoped<IMailHelper, MailHelper>();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LigaWeb API",
        Version = "v1",
        Description = "API para retornar a classificação do campeonato de futebol."
    });

    // Configurar autenticação JWT no Swagger (opcional)
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Insira o token JWT desta maneira: Bearer {seu_token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            securityScheme, new string[] { }
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});


var app = builder.Build();

// Aplicar migrações e realizar o seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try
    {
        // Chama o método de seeding
        await ApplicationDbContextSeed.SeedAsync(services);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Ocorreu um erro ao semear o banco de dados.");
    }
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Ativar Swagger somente em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LigaWeb API v1");
        c.RoutePrefix = "swagger"; // Swagger acessível via /swagger
    });
}

// Middleware de redirecionamento personalizado
app.Use(async (context, next) =>
{
    if (context.Request.Path.Equals("/index.html", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect("/");
        return;
    }
    await next();
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
