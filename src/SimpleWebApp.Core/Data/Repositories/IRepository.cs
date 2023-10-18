using SimpleWebApp.Core.Domain.Entities;

namespace SimpleWebApp.Core.Data.Repositories;

public interface IRepository<TKey, TEntity> where TEntity : IEntity<TKey>
{
    Task<TEntity?> GetAsync(TKey id);
    Task CreateAsync(TEntity entity);
}