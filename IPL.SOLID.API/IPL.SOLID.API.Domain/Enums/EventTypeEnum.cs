namespace IPL.SOLID.API.Domain.Enums;

[Flags]
public enum EventTypeEnum
{
    Creation = 1,
    Deletion = 2,
    Both = Creation | Deletion
}