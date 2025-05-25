namespace Domain.Interfaces;

public interface IEntity;

public interface IEntity<TKey> : IEntity
{
    TKey PKey { get; set; }
}

public interface IEntity<TKey1, TKey2> : IEntity
{
    TKey1 PKey1 { get; set; }
    TKey2 PKey2 { get; set; }
}