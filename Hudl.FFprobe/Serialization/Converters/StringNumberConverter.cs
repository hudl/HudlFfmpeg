using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hudl.FFmpeg.DataTypes;

namespace Hudl.FFprobe.Serialization.Converters;


internal abstract class BaseStringNumberConverter<TNumberType> : JsonConverter<TNumberType>
{
    protected abstract TNumberType? GetNumberValue(ref Utf8JsonReader reader);
    protected abstract TNumberType? GetNumberValueFromString(ref Utf8JsonReader reader);    

    public override bool CanConvert(Type typeToConvert) => typeof(TNumberType) == typeToConvert;

    public override TNumberType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => GetNumberValueFromString(ref reader) ?? default,
            JsonTokenType.Number => GetNumberValue(ref reader),
            _ => throw new Exception(
                $"Unexpected token parsing Number, expected {JsonTokenType.String} or {JsonTokenType.Number}, got {reader.TokenType}")
        };
    }

    public override void Write(Utf8JsonWriter writer, TNumberType value, JsonSerializerOptions options) =>
        throw new NotImplementedException("Unnecessary because CanWrite is false. the type will skip when converted");
}

internal class ShortNumberConverter : BaseStringNumberConverter<short>
{
    private const short DefaultShort = 0;

    protected override short GetNumberValue(ref Utf8JsonReader reader) => reader.TryGetInt16(out short value) ? value : DefaultShort;
    protected override short GetNumberValueFromString(ref Utf8JsonReader reader) => (short.TryParse(reader.GetString(), out var result) ? result : DefaultShort);
}

internal class IntNumberConverter : BaseStringNumberConverter<int>
{
    protected override int GetNumberValue(ref Utf8JsonReader reader) => reader.TryGetInt32(out int value) ? value : 0;
    protected override int GetNumberValueFromString(ref Utf8JsonReader reader) => (int.TryParse(reader.GetString(), out var result) ? result : 0);
}

internal class LongNumberConverter : BaseStringNumberConverter<long>
{
    protected override long GetNumberValue(ref Utf8JsonReader reader) => reader.TryGetInt64(out long value) ? value : 0L;
    protected override long GetNumberValueFromString(ref Utf8JsonReader reader) => (long.TryParse(reader.GetString(), out var result) ? result : 0L);
}