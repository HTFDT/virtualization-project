using Core.Utils;

namespace Services.Utils;

public static class EnvVars
{
    public static string S3KeyId => EnvUtils.GetEnv<string>("S3_KEY_ID");
    public static string S3SecretKey => EnvUtils.GetEnv<string>("S3_SECRET_KEY");
    public static string S3BucketName => EnvUtils.GetEnv<string>("S3_BUCKET_NAME");
}