using SimpleWebApp.Domain.Entities;

namespace SimpleWebApp.Data.SysLog.Mappers.Implementation;

public sealed class EmployeeSysLogMapper : ISysLogMapper<Guid, Employee>
{
    public SystemLog Map(Employee entity, SystemLogType systemLogType)
    {
        var comment = systemLogType switch
        {
            SystemLogType.Create => $"new employee {entity.Email} was created",
            SystemLogType.Update => $"employee {entity.Email} was update",
            _ => throw new NotImplementedException()
        };

        return new()
        {
            Id = $"Employee_{entity.Id}",
            Comment = comment,
            Event = systemLogType,
        };
    }
}
