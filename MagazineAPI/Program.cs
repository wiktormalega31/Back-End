using Microsoft.AspNetCore.Builder;
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
            Description = "Dokumentacja szkieletu RESTful API dla magazynu",
        }
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) // Uruchamianie Swaggera tylko w trybie dev
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        c.RoutePrefix = "swagger"; // Swagger dostępny pod /swagger
    });
}
app.Use(
    async (context, next) =>
    {
        // 🔹 Pobierz klucz API z nagłówka
        var apiKey = context.Request.Headers["X-API-KEY"].FirstOrDefault();
        var validApiKey = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5"; // 👈 Ustaw swój klucz API

        if (string.IsNullOrEmpty(apiKey) || apiKey != validApiKey)
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized: Invalid API Key");
            return;
        }

        await next(); // Przekaż żądanie do następnego middleware
    }
);

app.UseAuthorization();
app.MapControllers();

app.Run();
