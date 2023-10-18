namespace SimpleWebApp.Core.Domain.Entities;

public abstract class EntityBase<TKey> : IEntity<TKey>
{
    public virtual TKey? Id { get; set; }

    public virtual DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}