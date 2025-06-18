using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hudl.FFprobe.Metadata.Models;

namespace Hudl.FFprobe.Serialization.Converters;

internal class StreamConverter : JsonConverter<List<BaseStreamMetadata>>
{
    public override List<BaseStreamMetadata> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonArray = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options);

        return (from jsonElement in jsonArray
                let targetType = CodecTypesUtility.GetCodecTypeFromMediaType(jsonElement.GetProperty("codec_type").GetString()) switch
                {
                    CodecTypes.Video => typeof(VideoStreamMetadata),
                    CodecTypes.Audio => typeof(AudioStreamMetadata),
                    CodecTypes.Data => typeof(DataStreamMetadata),
                    _ => null,
                }
                select new { Type = targetType, Element = jsonElement })
            .Select(selected => JsonSerializer.Deserialize((string)selected.Element.GetRawText(), selected.Type, options))
            .Cast<BaseStreamMetadata>()
            .ToList(); 
    }

    public override void Write(Utf8JsonWriter writer, List<BaseStreamMetadata> value, JsonSerializerOptions options) =>
        throw new NotImplementedException("Unnecessary because CanWrite is false. the type will skip when converted");
}