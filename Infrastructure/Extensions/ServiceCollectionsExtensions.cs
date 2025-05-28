using Infrastructure.EFCore;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(EnvVars.ConnectionString));
        
        if (!EnvVars.ApplyMigrations)
            return services;
        var options = new DbContextOptionsBuilder()
            .UseNpgsql(EnvVars.ConnectionString)
            .Options;
        var context = new AppDbContext(options);
        context.Database.Migrate();
        
        return services;
    }
}