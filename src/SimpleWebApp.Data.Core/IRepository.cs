using SimpleWebApp.Domain.Core.Entities;

namespace SimpleWebApp.Data.Core;

public interface IRepository<TKey, TEntity> where TEntity : IEntity<TKey>
{
    Task<TEntity?> GetAsync(TKey id);
    Task CreateAsync(TEntity entity);
}