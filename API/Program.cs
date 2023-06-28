using Application;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Path.Combine(Directory.GetCurrentDirectory())).AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.Development.json", false)
            .AddEnvironmentVariables()
            .Build();

builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Information);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<CurrentAccountDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();