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

app.UseAuthorization();
app.MapControllers();

app.Run();
