namespace SimpleWebApp.Core.Domain.Entities;

public interface IEntity<TKey>
{
    public TKey? Id { get; }
    public DateTime CreatedAt { get; }
}