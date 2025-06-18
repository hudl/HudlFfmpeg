using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hudl.FFmpeg.DataTypes;

namespace Hudl.FFprobe.Serialization.Converters;

internal class FractionConverter : JsonConverter<Fraction>
{
    public override Fraction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new Exception($"Unexpected token parsing Fraction, expected String, got {reader.TokenType}");
        }

        _ = Fraction.TryParse(reader.GetString()!, out var fraction);

        return fraction;
    }

    public override void Write(Utf8JsonWriter writer, Fraction value, JsonSerializerOptions options) =>
        throw new NotImplementedException("Unnecessary because CanWrite is false. the type will skip when converted");
}