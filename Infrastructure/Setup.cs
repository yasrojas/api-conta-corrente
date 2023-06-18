using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.DbContext;

namespace Infrastructure;
public static class Setup
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Domain.DbContext.CurrentAccountDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgresCurrentAccountDb")));
    }

}
