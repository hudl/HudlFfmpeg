using System;
using System.Text.Json;
using Hudl.FFmpeg.DataTypes;
using System.Text.Json.Serialization;

namespace Hudl.FFprobe.Serialization.Converters;

internal class RatioConverter : JsonConverter<Ratio>
{
    public override Ratio? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new Exception($"Unexpected token parsing Ratio, expected {JsonTokenType.String}, got {reader.TokenType}");
        }

        _ = Ratio.TryParse(reader.GetString()!, out var ratio);

        return ratio;
    }
    public override void Write(Utf8JsonWriter writer, Ratio value, JsonSerializerOptions options) =>
        throw new NotImplementedException("Unnecessary because CanWrite is false. the type will skip when converted");
}