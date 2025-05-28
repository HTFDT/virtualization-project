using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces;
using Services.Services;
using Services.Utils;

namespace Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    { 
        var s3Config = new AmazonS3Config
        {
            ServiceURL = "https://s3.yandexcloud.net"
        };
        var credentials = new BasicAWSCredentials(EnvVars.S3KeyId, EnvVars.S3SecretKey);
        var client = new AmazonS3Client(credentials, s3Config);

        services.AddSingleton(client);
        services.AddScoped<IFileManager, S3FileManager>();
        services.AddScoped<IThreadsService, DefaultThreadsService>();
        services.AddScoped<IRatingsService, DefaultRatingsService>();
        return services;
    }
}
