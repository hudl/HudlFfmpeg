using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hudl.FFmpeg.DataTypes;

namespace Hudl.FFprobe.Serialization.Converters
{
    internal class RatioConverter : JsonConverter<Ratio>
    {
        //public override bool CanConvert(Type objectType)
        //{
        //    return objectType == typeof(string);
        //}

        //public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //{
        //    if (reader.TokenType != JsonToken.String)
        //    {
        //        throw new Exception(string.Format("Unexpected token parsing Ratio, expected String, got {0}", reader.TokenType));
        //    }

        //    Ratio ratio;

        //    Ratio.TryParse(reader.Value.ToString(), out ratio);

        //    return ratio;
        //}

        //public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //{

        //    throw new NotImplementedException("Unnecessary because CanWrite is false. the type will skip when converted");
        //}

        public override Ratio Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new Exception(string.Format("Unexpected token parsing Fraction, expected String, got {0}", reader.TokenType));
            }

            if (!Ratio.TryParse(reader.GetString(), out Ratio ratio))
            {
                return null;
            }

            return ratio;
        }

        public override void Write(Utf8JsonWriter writer, Ratio value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

    }
}
