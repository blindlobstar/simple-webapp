using SimpleWebApp.Core.Domain.Entities;

namespace SimpleWebApp.Data.SysLog.Mappers;

public interface ISysLogMapper<TKey, TEntity> where TEntity : IEntity<TKey>
{
    SystemLog Map(TEntity entity, SystemLogType systemLogType);
}