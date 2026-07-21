using API.DependencyInjection;
using API.Extensions;
using API.Middleware;
using Application.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Persistence.Extensions;
using Serilog;

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
// Add Authorization
builder.Services.AddCustomAuthorization();
// Add Serilog
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext();
});

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
