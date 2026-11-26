namespace IPL.SOLID.API.Domain.Enums;

public class ContentTypeSmartEnum
{
    public static readonly ContentTypeSmartEnum Xml = new("application/xml");
    public static readonly ContentTypeSmartEnum Json = new("application/json");
    public static readonly ContentTypeSmartEnum PlainText = new("application/plaintext");

    public string Value { get; }

    private ContentTypeSmartEnum(string value)
    {
        Value = value;
    }

    protected ContentTypeSmartEnum()
    {
        Value = string.Empty;
    }

    public static readonly IReadOnlyList<ContentTypeSmartEnum> All =
    [
        Xml, Json, PlainText
    ];

    public override string ToString() => Value;

    public static ContentTypeSmartEnum FromValue(string value)
    {
        return All.FirstOrDefault(x => x.Value == value)
               ?? throw new ArgumentOutOfRangeException(nameof(value), $"Invalid media type: {value}");
    }
}