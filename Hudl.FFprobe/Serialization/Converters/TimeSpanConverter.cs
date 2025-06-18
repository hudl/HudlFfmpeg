using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hudl.FFprobe.Serialization.Converters
{
    internal class TimeSpanConverter : JsonConverter<TimeSpan>
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

        //    double timespan;

        //    double.TryParse(reader.Value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out timespan);

        //    return TimeSpan.FromSeconds(timespan);
        //}

        //public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //{

        //    throw new NotImplementedException("Unnecessary because CanWrite is false. the type will skip when converted");
        //}

        //public override bool CanWrite
        //{
        //    get { return false; }
        //}
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new Exception(string.Format("Unexpected token parsing Ratio, expected String, got {0}", reader.TokenType));
            }

            if (!double.TryParse(reader.GetString(), NumberStyles.Number, CultureInfo.InvariantCulture, out double timespan)) 
            {
                return TimeSpan.Zero;
            }

            return TimeSpan.FromSeconds(timespan);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
