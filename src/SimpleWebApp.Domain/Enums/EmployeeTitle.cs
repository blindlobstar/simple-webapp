using System.Runtime.Serialization;

namespace SimpleWebApp.Domain.Enums;

public enum EmployeeTitle
{
    None = 0,

    [EnumMember(Value = "DEVELOPER")]
    Developer = 1,

    [EnumMember(Value = "MANAGER")]
    Manager = 2,

    [EnumMember(Value = "TESTER")]
    Tester = 3
}