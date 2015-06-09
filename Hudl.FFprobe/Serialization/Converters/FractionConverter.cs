using System;
using Hudl.FFmpeg.DataTypes;
using Newtonsoft.Json;

namespace Hudl.FFprobe.Serialization.Converters
{
    internal class FractionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception(string.Format("Unexpected token parsing Fraction, expected String, got {0}", reader.TokenType));
            }

            Fraction fraction;

            Fraction.TryParse(reader.Value.ToString(), out fraction);

            return fraction;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            throw new NotImplementedException("Unnecessary because CanWrite is false. the type will skip when converted");
        }

        public override bool CanWrite
        {
            get { return false; }
        }
    }
}
