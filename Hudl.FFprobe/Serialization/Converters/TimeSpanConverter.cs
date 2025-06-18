using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hudl.FFprobe.Serialization.Converters;

internal class TimeSpanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new Exception($"Unexpected token parsing Ratio, expected {JsonTokenType.String}, got {reader.TokenType}");
        }

        _ = double.TryParse(reader.GetString(), NumberStyles.Number, CultureInfo.InvariantCulture, out var timespan);
            
        return TimeSpan.FromSeconds(timespan);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options) =>
        throw new NotImplementedException("Unnecessary because CanWrite is false. the type will skip when converted");
}