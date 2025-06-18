using System.Collections.Generic;
using System.Text.Json.Serialization;
using Hudl.FFprobe.Serialization.Converters;

namespace Hudl.FFprobe.Metadata.Models;

public class ContainerMetadata
{
    [JsonPropertyName("format")]
    public FormatMetadata Format { get; set; }

    [JsonPropertyName("streams")]
    [JsonConverter(typeof(StreamConverter))]
    public List<BaseStreamMetadata> Streams { get; set; }

    [JsonPropertyName("frames")]
    [JsonConverter(typeof(FrameConverter))]
    public List<BaseFrameMetadata> Frames { get; set; }
}