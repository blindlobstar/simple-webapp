using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleWebApp.Core.Data.Repositories;
using SimpleWebApp.Core.Domain.Entities;
using SimpleWebApp.Data.SysLog.Mappers;

namespace SimpleWebApp.Data.SysLog.Repositories;

public abstract class LogRepository<TKey, TEntity> : EFRepositoryBase<TKey, TEntity>
    where TEntity : class, IEntity<TKey>
{
    private readonly ILogger<LogRepository<TKey, TEntity>> _logger;
    private readonly ISysLogMapper<TKey, TEntity> _sysLogMapper;
    protected LogRepository(
        DbContext dBContext,
        ILogger<LogRepository<TKey, TEntity>> logger,
        ISysLogMapper<TKey, TEntity> sysLogMapper) : base(dBContext)
    {
        _logger = logger;
        _sysLogMapper = sysLogMapper;
    }

    public override async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await base.CreateAsync(entity, cancellationToken);
        try
        {
            var changeSet = JsonSerializer.Serialize(entity, options: new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
            var systemLog = _sysLogMapper.Map(entity, SystemLogType.Create);
            systemLog.ChangeSet = changeSet;

            await DBContext.Set<SystemLog>().AddAsync(systemLog, cancellationToken);
            await DBContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding system entity");
            throw;
        }
    }
}