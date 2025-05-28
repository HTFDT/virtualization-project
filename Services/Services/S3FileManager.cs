using Amazon.S3;
using Amazon.S3.Model;
using Services.Interfaces;
using Services.Utils;

namespace Services.Services;

internal class S3FileManager(AmazonS3Client client) : IFileManager
{
    public Task SaveFileAsync(string key, Stream stream)
    {
        return client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = EnvVars.S3BucketName,
            Key = key,
            InputStream = stream
        });
    }

    public async Task<Stream> GetFileAsync(string key)
    {
        var response = await client.GetObjectAsync(EnvVars.S3BucketName, key);
        return response.ResponseStream;
    }

    public Task DeleteFileAsync(string key)
    {
        return client.DeleteObjectAsync(EnvVars.S3BucketName, key);
    }
}