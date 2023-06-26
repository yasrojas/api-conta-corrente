using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class Setup
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CurrentAccountDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgresCurrentAccountDb")));
    }

}
