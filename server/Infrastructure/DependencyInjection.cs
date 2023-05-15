using Application.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        return services
            .AddDb(config)
            .AddTransient<IIdentityService, IdentityService>();
    }
    
    private static IServiceCollection AddDb(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        });
        return services;
    }
}