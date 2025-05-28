using Core.Exceptions;

namespace Core.Utils;

public static class EnvUtils
{
    public static TEnv GetEnv<TEnv>(string key)
    {
        var env = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrEmpty(env))
            throw new EnvVarNotFoundException(key);

        if (typeof(TEnv) == typeof(string))
        {
            return (TEnv)(object)env;
        }
        if (typeof(TEnv) == typeof(int))
        {
            return (TEnv)(object)int.Parse(env);
        }
        if (typeof(TEnv) == typeof(bool))
        {
            return (TEnv)(object)bool.Parse(env);
        }
        throw new NotSupportedException($"{typeof(TEnv)} is not supported");
    }
}