using Infrastructure.Data;
using Application;
using API.Middleware;

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
// For MiddelWear
builder.Services.AddProblemDetails();
// Infrastructure DependecyInjection
builder.Services.AddInfrastructure(builder.Configuration);
// Application DependecyInjection
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthorization();
app.MapControllers();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
