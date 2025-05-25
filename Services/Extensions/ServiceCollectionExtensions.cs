using Microsoft.Extensions.DependencyInjection;

namespace Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
}