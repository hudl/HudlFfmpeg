using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes.Converters;
using Newtonsoft.Json;

namespace Hudl.FFmpeg.Metadata.FFprobe.BaseTypes.Models
{
    [JsonObject]
    public class FormatMetadata
    {
        [JsonProperty(PropertyName = "filename")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "nb_streams")]
        public int NumberOfStreams { get; set; }

        [JsonProperty(PropertyName = "nb_programs")]
        public int NumberOfPrograms { get; set; }

        [JsonProperty(PropertyName = "format_name")]
        public string FormatName { get; set; }

        [JsonProperty(PropertyName = "format_long_name")]
        public string FormatLongName { get; set; }

        [JsonProperty(PropertyName = "start_time")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan StartTime { get; set; }

        [JsonProperty(PropertyName = "duration")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Duration { get; set; }

        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; }

        [JsonProperty(PropertyName = "bit_rate")]
        public long BitRate { get; set; }

        [JsonProperty(PropertyName = "probe_score")]
        public int ProbeScore { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public Dictionary<string, string> Tags { get; set; } 
    }
}
