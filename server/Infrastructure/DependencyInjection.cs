using Application.Features.Dividend.Interfaces;
using Application.Interfaces;
using Ardalis.Specification;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Providers.Morningstar;
using Infrastructure.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        return services
            .AddDb(config)
            .AddHostedService(sp => new PeriodicHostedService(
                sp, sp.GetRequiredService<ILogger<PeriodicHostedService>>()))
            .AddScoped(typeof(IRepositoryBase<>), typeof(DbSetRepository<>))
            .AddScoped(typeof(IReadRepositoryBase<>), typeof(DbSetRepository<>))
            .AddAutoMapper(typeof(DependencyInjection).Assembly)
            .AddTransient(typeof(ISecuritiesInfoProvider), typeof(SecuritiesInfoProvider))
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