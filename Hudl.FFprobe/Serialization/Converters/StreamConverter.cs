using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;
using Hudl.FFprobe.Metadata.Models;

namespace Hudl.FFprobe.Serialization.Converters
{
    internal class StreamConverter : JsonConverter<List<BaseStreamMetadata>>
    {


        //public BaseStreamMetadata Create(Type objectType, JObject jsonObject)
        //{
        //    var codecType = (string)jsonObject.Property("codec_type");
        //    if (string.Equals(codecType, CodecTypes.Video.ToString(), StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        return new VideoStreamMetadata();
        //    }

        //    if (string.Equals(codecType, CodecTypes.Audio.ToString(), StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        return new AudioStreamMetadata();
        //    }

        //    if (string.Equals(codecType, CodecTypes.Data.ToString(), StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        return new DataStreamMetadata();
        //    }

        //    return null; 
        //}

        //public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //{
        //    var jsonArray = JArray.Load(reader);

        //    var returnList = new List<BaseStreamMetadata>();

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
        public override List<BaseStreamMetadata> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Expected start of array token.");
            }

            //search for the codec type property
            var streams = new List<BaseStreamMetadata>();
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                reader.Read();
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
                    if (propertyName != "codec_type")
                    {
                        continue;
                    }

                    readerClone.Read();
                    if (readerClone.TokenType != JsonTokenType.String)
                    {
                        throw new JsonException($"expected codec_type to be a string, instead got {readerClone.TokenType.ToString()}");
                    }

                    var codecType = readerClone.GetString();
                    if (string.Equals(codecType, CodecTypes.Video.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        streams.Add(JsonSerializer.Deserialize<VideoStreamMetadata>(ref reader, options));
                    }
                    else if (string.Equals(codecType, CodecTypes.Audio.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        streams.Add(JsonSerializer.Deserialize<AudioStreamMetadata>(ref reader, options));
                    }
                    else if (string.Equals(codecType, CodecTypes.Data.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        streams.Add(JsonSerializer.Deserialize<DataStreamMetadata>(ref reader, options));
                    }
                }
            }
            return streams; 
        }

        public override void Write(Utf8JsonWriter writer, List<BaseStreamMetadata> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
