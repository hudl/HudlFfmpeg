using System.Collections.Generic;
using Hudl.FFprobe.Serialization;
using Hudl.FFprobe.Serialization.Converters;
using Newtonsoft.Json;

namespace Hudl.FFprobe.Metadata.Models
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
