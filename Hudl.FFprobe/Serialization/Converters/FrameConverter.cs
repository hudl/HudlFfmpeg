using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hudl.FFprobe.Metadata.Models;

namespace Hudl.FFprobe.Serialization.Converters;

internal class FrameConverter : JsonConverter<List<BaseFrameMetadata>>
{
    public override List<BaseFrameMetadata>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonArray = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options);

        return (from jsonElement in jsonArray
                let targetType = CodecTypesUtility.GetCodecTypeFromMediaType(jsonElement.GetProperty("media_type").GetString()) switch
                {
                    CodecTypes.Video => typeof(VideoFrameMetadata),
                    CodecTypes.Audio => typeof(AudioFrameMetadata),
                    _ => null,
                }
                select new { Type = targetType, Element = jsonElement })
            .Select(selected => JsonSerializer.Deserialize((string)selected.Element.GetRawText(), selected.Type, options))
            .Cast<BaseFrameMetadata>()
            .ToList(); 
    }

    public override void Write(Utf8JsonWriter writer, List<BaseFrameMetadata> value, JsonSerializerOptions options) =>
        throw new NotImplementedException("Unnecessary because CanWrite is false. the type will skip when converted");
}