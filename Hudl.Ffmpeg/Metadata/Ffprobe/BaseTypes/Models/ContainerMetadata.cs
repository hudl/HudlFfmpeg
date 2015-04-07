using System.Collections.Generic;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes.Converters;
using Newtonsoft.Json;

namespace Hudl.FFmpeg.Metadata.FFprobe.BaseTypes.Models
{
    [JsonObject]
    public class ContainerMetadata
    {
        [JsonProperty(PropertyName = "format")]
        public FormatMetadata Format { get; set; }

        [JsonProperty(PropertyName = "streams")]
        [JsonConverter(typeof(StreamConverter))]
        public List<BaseStreamMetadata> Streams { get; set; }
    }
}
