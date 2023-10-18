using System.Runtime.Serialization;

namespace SimpleWebApp.Data.SysLog;

public enum SystemLogType
{
    [EnumMember(Value = "CREATED")]
    Create,

    [EnumMember(Value = "UPDATED")]
    Update
}