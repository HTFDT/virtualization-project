namespace Services.Utils.ResponseFactory.Interfaces;

public interface IBuilder<out T>
{
    T Build();
}