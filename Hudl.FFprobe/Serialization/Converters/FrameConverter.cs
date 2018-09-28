﻿using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;
using Hudl.FFprobe.Metadata.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Hudl.FFprobe.Serialization.Converters
{
    internal class FrameConverter : CustomCreationConverter<List<BaseFrameMetadata>>
    {
        public override List<BaseFrameMetadata> Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public BaseFrameMetadata Create(Type objectType, JObject jsonObject)
        {
            var codecType = (string)jsonObject.Property("media_type");
            if (string.Equals(codecType, CodecTypes.Video.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return new VideoFrameMetadata();
            }
             
            if (string.Equals(codecType, CodecTypes.Audio.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return new AudioFrameMetadata();
            }

            if (string.Equals(codecType, CodecTypes.Subtitle.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return new SubtitleFrameMetadata();
            }

            return null; 
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonArray = JArray.Load(reader);

            var returnList = new List<BaseFrameMetadata>();

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
