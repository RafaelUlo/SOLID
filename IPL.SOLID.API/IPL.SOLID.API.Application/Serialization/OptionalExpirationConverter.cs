using System.Text.Json;
using System.Text.Json.Serialization;

namespace IPL.SOLID.API.Application.Serialization;

public sealed class OptionalExpirationConverter : JsonConverter<DateTimeOffset?>
{
    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            if (reader.GetString() == string.Empty)
            {
                return null;
            }

            if (reader.TryGetDateTimeOffset(out var expiration))
            {
                return expiration;
            }
        }

        throw new JsonException("dt-expiration must be an ISO date-time or empty.");
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
