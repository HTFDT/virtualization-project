namespace Services.Interfaces;

public interface IFileManager
{
    Task SaveFileAsync(string key, Stream stream);
    Task<Stream> GetFileAsync(string key);
    Task DeleteFileAsync(string key);
}