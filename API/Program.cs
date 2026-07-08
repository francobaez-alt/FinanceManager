using API.DependencyInjection;
using API.Middleware;
using Application.Common;
using Infrastructure.Data;
using Infrastructure.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Exception Handling Middleware
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Swagger
builder.Services.AddSwaggerGen();
// Infrastructure DependecyInjection
builder.Services.AddInfrastructure(builder.Configuration);
// Application DependecyInjection
builder.Services.AddApplication();
// Add Authentication and Authorization
builder.Services.AddJwtAuthentication(
    builder.Configuration);
// Add Validation
builder.Services.AddValidation();

var app = builder.Build();

await app.Services.InitializeDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.MapControllers();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
