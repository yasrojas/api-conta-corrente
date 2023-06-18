using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class Setup
    {
        public static void AddApplication(this IServiceCollection services)
        {
            Assembly assembly = typeof(Setup).Assembly;

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

            services.AddValidatorsFromAssembly(assembly);
        }
    }
}