namespace Core.Exceptions;

public class EnvVarNotFoundException(string key) : Exception($"Variable \"{key}\" not found or empty.");