using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hudl.FFprobe.Serialization.Converters;

internal class StringToLongConverter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new Exception(string.Format("Unexpected token parsing long, expected String, got {0}", reader.TokenType));
        }

        if (!long.TryParse(reader.GetString(), out long value))
        {
            return 0L;
        }

        return value;
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
