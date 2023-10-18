using SimpleWebApp.Domain.Core.Entities;

namespace SimpleWebApp.Data.SysLog;

public sealed class SystemLog : EntityBase<string>
{
    public SystemLogType Event { get; set; }
    public string ChangeSet { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
}