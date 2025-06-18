using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hudl.FFmpeg.DataTypes;

namespace Hudl.FFprobe.Serialization.Converters
{

    internal class FractionConverter : JsonConverter<Fraction>
    {
        public override Fraction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new Exception(string.Format("Unexpected token parsing Fraction, expected String, got {0}", reader.TokenType));
            }

            if (!Fraction.TryParse(reader.GetString(), out Fraction fraction))
            {
                return null;
            }

            return fraction;
        }

        public override void Write(Utf8JsonWriter writer, Fraction value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
