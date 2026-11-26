using IPL.SOLID.API.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IPL.SOLID.API.Infrastructure.SQLite.Converters;

public class ContentTypeSmartEnumConverter : ValueConverter<ContentTypeSmartEnum, string>
{
    public ContentTypeSmartEnumConverter()
        : base(
            v => v.Value,
            v => ContentTypeSmartEnum.FromValue(v)
        )
    { }
}
