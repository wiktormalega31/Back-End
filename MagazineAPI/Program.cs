using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

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
});

// 🔹 Dodanie obsługi CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// 🔹 Włączenie Swaggera
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    c.RoutePrefix = "swagger"; // Swagger dostępny pod /swagger
});

// 🔹 Włączenie CORS
app.UseCors();

// 🔹 Middleware do weryfikacji klucza API
app.Use(
    async (context, next) =>
    {
        var apiKey = context.Request.Headers["X-API-KEY"].FirstOrDefault();
        var validApiKey = "1234"; // 👈 Ustaw swój klucz API

        if (string.IsNullOrEmpty(apiKey) || apiKey != validApiKey)
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized: Invalid API Key");
            return;
        }

        await next(); // Przekaż żądanie do kolejnego middleware
    }
);

app.UseAuthorization();
app.MapControllers();
app.Run();
