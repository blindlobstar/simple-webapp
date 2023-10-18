using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Core.Domain.Entities;

namespace SimpleWebApp.Core.Data.Repositories;

public abstract class EFRepositoryBase<TKey, TEntity> : IRepository<TKey, TEntity>
    where TEntity : class, IEntity<TKey>
{
    protected DbContext DBContext { get; private set; }
    protected DbSet<TEntity> DbSet => DBContext.Set<TEntity>();

    protected EFRepositoryBase(DbContext dBContext)
    {
        DBContext = dBContext;
    }

    public virtual async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await DBContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken: cancellationToken);
}
