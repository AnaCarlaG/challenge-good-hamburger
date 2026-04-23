using Application.Service;
using Application.Service.Interface;
using Domain.Exceptions;
using Domain.Interfaces.Repository;
using Infrastructure.Context;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//DbContext
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//Repositories
builder.Services.AddScoped<IPedidoRepository,PedidoRepository>();
builder.Services.AddScoped<IItemCardapioRepository, ItemCardapioRepository>();
//Services
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IItemCardapioService,ItemCardapioService>();

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware de erros
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    var (statusCode, message) = exception switch
    {
        NotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
        PedidoInvalidoException ex => (StatusCodes.Status422UnprocessableEntity, ex.Message),
        _ => (StatusCodes.Status500InternalServerError, "Ocorreu um erro interno.")
    };
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync( new { 
            status = statusCode,
            message
        });
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

// Seed do banco
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}
app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
