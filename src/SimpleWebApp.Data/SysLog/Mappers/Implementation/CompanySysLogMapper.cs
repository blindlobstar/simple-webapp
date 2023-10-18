using SimpleWebApp.Domain.Entities;

namespace SimpleWebApp.Data.SysLog.Mappers.Implementation;

public sealed class CompanySysLogMapper : ISysLogMapper<Guid, Company>
{
    public SystemLog Map(Company entity, SystemLogType systemLogType)
    {
        var comment = systemLogType switch
        {
            SystemLogType.Create => $"new company {entity.Name} was created",
            SystemLogType.Update => $"company {entity.Name} was update",
            _ => throw new NotImplementedException()
        };

        return new()
        {
            Id = $"Company_{entity.Id}",
            Comment = comment,
            Event = systemLogType,
        };
    }
}
