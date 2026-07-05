using Infrastructure.Data;
using Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

app.UseAuthorization();

app.MapControllers();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
