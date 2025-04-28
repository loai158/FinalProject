using FinalProject.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.DependencyInjection;

namespace FinalProject.Infrastructure
{
    public static class InfrastructureDependencies
    { 
            public static IServiceCollection AddInfrastructureDependences(this IServiceCollection services)
            {
                services.AddScoped<IUnitOfWork, UnitOfWork>();
                return services;
            }
       
    }
}
