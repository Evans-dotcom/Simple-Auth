using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using UserAuthProject.Data;
using UserAuthProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<AuthService>();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline for both Development and Production (IIS)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Set the Swagger endpoint dynamically for IIS or local development
    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
    c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "My API V1");

    // Use the base path for hosting in IIS if necessary (e.g., /Simple_Auth)
    c.RoutePrefix = "swagger"; // Keep this empty for IIS root, or set it as "swagger" if needed
});

app.UseAuthorization();

// Handle IIS virtual directory (if the app is hosted in IIS under /Simple_Auth)
app.UsePathBase("/Simple_Auth");

app.MapControllers();

app.Run();
