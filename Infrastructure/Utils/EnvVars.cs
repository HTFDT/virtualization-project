using Core.Utils;

namespace Infrastructure.Utils;

public static class EnvVars
{
    public static string ConnectionString => EnvUtils.GetEnv<string>("CONNECTION_STRING");
    public static bool ApplyMigrations => EnvUtils.GetEnv<bool>("APPLY_MIGRATIONS");
}