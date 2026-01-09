using System.Text;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Ajout des services au conteneur ---

// Configuration OpenAPI / Swagger
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Base de données et Repositories
builder.Services.AddDbContext<UserContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("GamerHistory")));
builder.Services.AddScoped<UserContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Ajout des nouveaux repositories pour les jeux et supports
builder.Services.AddScoped<ISupportRepository, SupportRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
// Service JWT (que tu as créé dans Infrastructure)
builder.Services.AddScoped<JwtService>();

// Configuration CORS
const string corsName = "MyOrigins"; 
builder.Services.AddCors(options => 
{ 
    options.AddPolicy(corsName, cpb => 
    {  
        cpb.WithOrigins("http://localhost:4200")
            .AllowAnyMethod() 
            .AllowAnyHeader()
            .AllowCredentials();
    }); 
}); 

// --- CONFIGURATION JWT BEARER ---
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

// Petite sécurité pour éviter de planter si la config est vide
if (string.IsNullOrEmpty(jwtKey)) 
{
    throw new Exception("Attention : La clé 'Jwt:Key' est introuvable dans appsettings.json !");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };

    // Configuration pour lire le token depuis le Cookie "jwt"
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("jwt"))
            {
                context.Token = context.Request.Cookies["jwt"];
            }
            return Task.CompletedTask;
        }
    };
});
// ----------------------------------

var app = builder.Build();

// --- 2. Configuration du pipeline HTTP ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(corsName); 

// --- ORDRE IMPORTANT DES MIDDLEWARES DE SÉCURITÉ ---
// Doit être placé APRÈS UseCors et AVANT MapControllers
app.UseAuthentication(); // "Qui est-ce ?" (Vérifie le cookie/token)
app.UseAuthorization();  // "A-t-il le droit ?" (Vérifie le rôle)

app.MapControllers();

app.Run();