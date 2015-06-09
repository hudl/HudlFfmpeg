using System;
using System.Collections.Generic;
using Hudl.FFmpeg.DataTypes;
using Hudl.FFprobe.Serialization;
using Hudl.FFprobe.Serialization.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hudl.FFprobe.Metadata.Models
{
    [JsonObject]
    public class BaseStreamMetadata
    {
        public BaseStreamMetadata()
        {
            AdditionalData = new Dictionary<string, JToken>();
        }

        [JsonProperty(PropertyName = "index")]
        public int Index { get; set; }

        [JsonProperty(PropertyName = "codec_name")]
        public string CodecName { get; set; }

        [JsonProperty(PropertyName = "codec_long_name")]
        public string CodecLongName { get; set; }

        [JsonProperty(PropertyName = "codec_type")]
        public string CodecType { get; set; }

        [JsonProperty(PropertyName = "codec_time_base")]
        [JsonConverter(typeof(FractionConverter))]
        public Fraction CodecTimeBase { get; set; }

        [JsonProperty(PropertyName = "codec_tag_string")]
        public string CodecTagString { get; set; }

        [JsonProperty(PropertyName = "codec_tag")]
        public string CodecTag { get; set; }

        [JsonProperty(PropertyName = "profile")]
        public string Profile { get; set; }

        [JsonProperty(PropertyName = "time_base")]
        [JsonConverter(typeof(FractionConverter))]
        public Fraction TimeBase { get; set; }

        [JsonProperty(PropertyName = "bit_rate")]
        public long BitRate { get; set; }

        [JsonProperty(PropertyName = "start_pts")]
        public long StartTimeTs { get; set; }

        [JsonProperty(PropertyName = "start_time")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan StartTime { get; set; }

        [JsonProperty(PropertyName = "duration_ts")]
        public long DurationTs { get; set; }

        [JsonProperty(PropertyName = "duration")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Duration { get; set; }

        [JsonProperty(PropertyName = "nb_frames")]
        public int NumberOfFrames { get; set; }

        [JsonProperty(PropertyName = "disposition")]
        public Dictionary<string, string> Disposition { get; set; } 

        [JsonProperty(PropertyName = "tags")]
        public Dictionary<string, string> Tags { get; set; } 

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData;
    }

}
