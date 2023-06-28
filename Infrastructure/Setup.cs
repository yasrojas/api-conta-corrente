using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class Setup
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<CurrentAccountDbContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("PostgresCurrentAccountDb")));
        services.AddScoped<ICurrentAccountDbContext, CurrentAccountDbContext>();
        services.AddScoped<ICurrentAccountRepository, CurrentAccountRepository>();
    }

    
}
