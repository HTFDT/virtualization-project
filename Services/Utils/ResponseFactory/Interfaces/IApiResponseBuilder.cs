namespace Services.Utils.ResponseFactory.Interfaces;

public interface IApiResponseBuilder<out TBuilder, out T> : IBuilder<T> where TBuilder: IBuilder<T>
{
    TBuilder NoContent();
    TBuilder Success();
    TBuilder Error(int statusCode, string? message = null);
}

public interface IApiResponseBuilder<out T> : IBuilder<T>
{
    void NoContent();
    void Success();
    void Error(int statusCode, string? message = null);
}