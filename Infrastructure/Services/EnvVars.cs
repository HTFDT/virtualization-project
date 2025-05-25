using Core.Utils;

namespace Infrastructure.Services;

public static class EnvVars
{
    public static string ConnectionString => EnvUtils.GetEnv<string>("CONNECTION_STRING");
}