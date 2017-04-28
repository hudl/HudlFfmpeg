using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;
using Hudl.FFprobe.Metadata.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Hudl.FFprobe.Serialization.Converters
{
    internal class StreamConverter : CustomCreationConverter<List<BaseStreamMetadata>>
    {
        public override List<BaseStreamMetadata> Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public BaseStreamMetadata Create(Type objectType, JObject jsonObject)
        {
            var codecType = (string)jsonObject.Property("codec_type");
            if (string.Equals(codecType, CodecTypes.Video.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return new VideoStreamMetadata();
            }
             
            if (string.Equals(codecType, CodecTypes.Audio.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return new AudioStreamMetadata();
            }

            if (string.Equals(codecType, CodecTypes.Data.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return new DataStreamMetadata();
            }

            return null; 
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonArray = JArray.Load(reader);

            var returnList = new List<BaseStreamMetadata>();

            foreach (var jsonToken in jsonArray)
            {
                if (jsonToken.Type != JTokenType.Object)
                {
                    throw new Exception(string.Format("Expected a token type of Object, got {0} instead", jsonToken.Type));
                }

                var targetObject = jsonToken.Value<JObject>();
                var targetType = Create(objectType, targetObject);
                if (targetType == null)
                {
                    //unsupported type, dont wanna worry about it.
                    continue;
                }

                serializer.Populate(targetObject.CreateReader(), targetType);

                returnList.Add(targetType);
            }

            return returnList; 
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
