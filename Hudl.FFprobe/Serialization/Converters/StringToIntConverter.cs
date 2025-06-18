using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hudl.FFprobe.Serialization.Converters;

internal class StringToIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new Exception(string.Format("Unexpected token parsing int, expected String, got {0}", reader.TokenType));
        }

        if (!int.TryParse(reader.GetString(), out int value))
        {
            return 0;
        }

        return value;
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
