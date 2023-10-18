using Microsoft.Extensions.DependencyInjection;
using SimpleWebApp.Core.Data.Repositories;
using SimpleWebApp.Core.Domain.Entities;
using SimpleWebApp.Data.SysLog.Mappers;
using SimpleWebApp.Data.SysLog.Repositories;

namespace SimpleWebApp.Data.SysLog;

public static class Extensions
{
    public static IServiceCollection AddLoggedRepository<TRepository, TRepositoryImplementation, TMapper, TEntity, TKey>(this IServiceCollection services)
        where TRepository : class, IRepository<TKey, TEntity>
        where TRepositoryImplementation : LogRepository<TKey, TEntity>, TRepository
        where TMapper : class, ISysLogMapper<TKey, TEntity>
        where TEntity : class, IEntity<TKey>
    {
        services.AddTransient<TRepository, TRepositoryImplementation>();
        services.AddSingleton<ISysLogMapper<TKey, TEntity>, TMapper>();
        return services;
    }
}