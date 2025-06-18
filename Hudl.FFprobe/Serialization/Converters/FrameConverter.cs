using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;
using Hudl.FFprobe.Metadata.Models;

namespace Hudl.FFprobe.Serialization.Converters
{
    internal class FrameConverter : JsonConverter<List<BaseFrameMetadata>>
    {
        //public override List<BaseFrameMetadata> Create(Type objectType)
        //{
        //    throw new NotImplementedException();
        //}

        //public BaseFrameMetadata Create(Type objectType, JObject jsonObject)
        //{
        //    var codecType = (string)jsonObject.Property("media_type");
        //    if (string.Equals(codecType, CodecTypes.Video.ToString(), StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        return new VideoFrameMetadata();
        //    }

        //    if (string.Equals(codecType, CodecTypes.Audio.ToString(), StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        return new AudioFrameMetadata();
        //    }

        //    return null; 
        //}

        //public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //{
        //    var jsonArray = JArray.Load(reader);

        //    var returnList = new List<BaseFrameMetadata>();

        //    foreach (var jsonToken in jsonArray)
        //    {
        //        if (jsonToken.Type != JTokenType.Object)
        //        {
        //            throw new Exception(string.Format("Expected a token type of Object, got {0} instead", jsonToken.Type));
        //        }

        //        var targetObject = jsonToken.Value<JObject>();
        //        var targetType = Create(objectType, targetObject);
        //        if (targetType == null)
        //        {
        //            //unsupported type, dont wanna worry about it.
        //            continue;
        //        }

        //        serializer.Populate(targetObject.CreateReader(), targetType);

        //        returnList.Add(targetType);
        //    }

        //    return returnList; 
        //}

        //public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //{
        //    throw new NotImplementedException("Unnecessary because CanWrite is false. the type will skip when converted");
        //}

        //public override bool CanWrite
        //{
        //    get { return false; }
        //}

        public override List<BaseFrameMetadata> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Expected start of array token.");
            }

            //search for the codec type property
            var frames = new List<BaseFrameMetadata>();
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                reader.Read(); // Move to the first element in the array
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    continue;
                }

                Utf8JsonReader readerClone = reader;
                //search for the codec type property
                while (readerClone.TokenType != JsonTokenType.EndObject
                    && readerClone.TokenType != JsonTokenType.EndArray)
                {
                    readerClone.Read();
                    if (readerClone.TokenType != JsonTokenType.PropertyName)
                    {
                        continue;
                    }

                    var propertyName = readerClone.GetString();
                    if (propertyName != "media_type")
                    {
                        continue;
                    }

                    readerClone.Read();
                    if (readerClone.TokenType != JsonTokenType.String)
                    {
                        throw new JsonException($"expected media_type to be a string, instead got {readerClone.TokenType.ToString()}");
                    }

                    var codecType = readerClone.GetString();
                    if (string.Equals(codecType, CodecTypes.Video.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        frames.Add(JsonSerializer.Deserialize<VideoFrameMetadata>(ref reader, options));
                    }
                    else if (string.Equals(codecType, CodecTypes.Audio.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        frames.Add(JsonSerializer.Deserialize<AudioFrameMetadata>(ref reader, options));
                    }
                }
            }
            return frames;
        }

        public override void Write(Utf8JsonWriter writer, List<BaseFrameMetadata> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
