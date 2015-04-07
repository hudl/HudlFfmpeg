using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Hudl.FFmpeg.Metadata.FFprobe.BaseTypes.Converters
{
    internal class TimeSpanConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception(string.Format("Unexpected token parsing Ratio, expected String, got {0}", reader.TokenType));
            }

            double timespan;

            double.TryParse(reader.Value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out timespan);
            
            return TimeSpan.FromSeconds(timespan);
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
