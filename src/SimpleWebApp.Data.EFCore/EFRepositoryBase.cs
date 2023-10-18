using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Data.Core;
using SimpleWebApp.Domain.Core.Entities;

namespace SimpleWebApp.Data.EFCore;

public abstract class EFRepositoryBase<TKey, TEntity> : IRepository<TKey, TEntity> 
    where TEntity : class, IEntity<TKey> 
{
    protected DbContext DBContext { get; private set; }
    protected DbSet<TEntity> DbSet => DBContext.Set<TEntity>();

    protected EFRepositoryBase(DbContext dBContext)
    {
        DBContext = dBContext;
    }

    public virtual async Task CreateAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await DBContext.SaveChangesAsync();
    }

    public virtual async Task<TEntity?> GetAsync(TKey id) => 
        await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id!.Equals(id));
}