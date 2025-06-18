using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hudl.FFmpeg.DataTypes;

namespace Hudl.FFprobe.Serialization.Converters;

internal class NumberStringConverter : JsonConverter<string>
{
    public override bool CanConvert(Type typeToConvert) => typeof(string) == typeToConvert;

    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString() ?? default,
            JsonTokenType.Number => reader.TryGetInt64(out var value) ? value.ToString() : default,
            _ => throw new Exception(
                $"Unexpected token parsing String, expected {JsonTokenType.String} or {JsonTokenType.Number}, got {reader.TokenType}")
        };
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) =>
        throw new NotImplementedException("Unnecessary because CanWrite is false. the type will skip when converted");
}