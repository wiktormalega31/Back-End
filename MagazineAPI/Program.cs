using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Pobranie connection stringa
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Dodanie kontekstu bazy danych
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Pobranie konfiguracji JWT z appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
var keyString = jwtSettings["Key"];
if (string.IsNullOrEmpty(keyString))
{
    Console.WriteLine("Błąd: Klucz JWT nie został znaleziony w konfiguracji.");
    throw new ArgumentNullException("Jwt:Key", "Klucz JWT nie może być null lub pusty.");
}
Console.WriteLine(
    $"Klucz JWT został odczytany: {keyString.Substring(0, Math.Min(10, keyString.Length))}..."
);
var key = Encoding.UTF8.GetBytes(keyString);

// Walidacja długości klucza JWT
if (key.Length < 32)
{
    throw new ArgumentOutOfRangeException(
        "Key",
        "Klucz JWT musi mieć co najmniej 256 bitów (32 znaki w Base64)."
    );
}

// Włączenie wyświetlania szczegółowych informacji o błędach (PII)
IdentityModelEventSource.ShowPII = true;

// Konfiguracja JWT
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Błąd uwierzytelniania: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token został poprawnie zweryfikowany.");
                return Task.CompletedTask;
            },
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Back-End LAB test REST-API",
            Version = "v1",
            Description = "Dokumentacja szkieletu RESTful API dla obsługi logowania",
        }
    );

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Podaj token JWT",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                new string[] { }
            },
        }
    );
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    c.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
