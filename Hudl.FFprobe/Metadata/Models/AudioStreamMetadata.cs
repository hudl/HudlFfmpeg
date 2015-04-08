using Newtonsoft.Json;

namespace Hudl.FFprobe.Metadata.Models
{
    [JsonObject]
    public class AudioStreamMetadata : BaseStreamMetadata
    {
        [JsonProperty(PropertyName = "sample_fmt")]
        public string SampleFormat { get; set; }

        [JsonProperty(PropertyName = "sample_rate")]
        public int SampleRate { get; set; }

        [JsonProperty(PropertyName = "channels")]
        public int Channels { get; set; }

        [JsonProperty(PropertyName = "channel_layout")]
        public string ChannelLayout { get; set; }

        [JsonProperty(PropertyName = "bits_per_sample")]
        public int BitsPerSample { get; set; }

        public AudioStreamMetadata Copy()
        {
            return (AudioStreamMetadata)MemberwiseClone();
        }
    }
}
