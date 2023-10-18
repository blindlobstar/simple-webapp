namespace SimpleWebApp.Domain.Core.Entities;

public interface IEntity<TKey> 
{
    public TKey? Id { get; }
    public DateTime CreatedAt { get; }
}