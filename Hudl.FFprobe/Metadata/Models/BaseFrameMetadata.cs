using System;
using System.Collections.Generic;
using Hudl.FFprobe.Serialization.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hudl.FFprobe.Metadata.Models
{
    [JsonObject]
    public class BaseFrameMetadata
    {
        public BaseFrameMetadata()
        {
            AdditionalData = new Dictionary<string, JToken>();
        }

        [JsonProperty(PropertyName = "media_type")]
        public string MediaType { get; set; }

        [JsonProperty(PropertyName = "stream_index")]
        public int StreamIndex { get; set; }

        [JsonProperty(PropertyName = "key_frame")]
        public long KeyFrame { get; set; }

        [JsonProperty(PropertyName = "pkt_pts")]
        public long PacketPresentationTimestamp { get; set; }

        [JsonProperty(PropertyName = "pkt_pts_time")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan PacketPresentationTimestampTime { get; set; }

        [JsonProperty(PropertyName = "pkt_dts")]
        public long PacketDecodingTimestamp { get; set; }

        [JsonProperty(PropertyName = "pkt_dts_time")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan PacketDecodingTimestampTime { get; set; }

        [JsonProperty(PropertyName = "pkt_duration")]
        public long PacketDuration { get; set; }

        [JsonProperty(PropertyName = "pkt_duration_time")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan PacketDurationTime { get; set; }

        [JsonProperty(PropertyName = "best_effort_timestamp")]
        public long BestEffortTimestamp { get; set; }

        [JsonProperty(PropertyName = "best_effort_timestamp_time")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan BestEffortTimestampTime { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData;
    }

}
