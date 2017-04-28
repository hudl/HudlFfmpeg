using Newtonsoft.Json;

namespace Hudl.FFprobe.Metadata.Models
{
    [JsonObject]
    public class AudioFrameMetadata : BaseFrameMetadata
    {
        [JsonProperty(PropertyName = "sample_fmt")]
        public string SampleFormat { get; set; }

        [JsonProperty(PropertyName = "channels")]
        public int Channels { get; set; }

        [JsonProperty(PropertyName = "channel_layout")]
        public string ChannelLayout { get; set; }

        [JsonProperty(PropertyName = "nb_samples")]
        public int NumberOfSamples { get; set; }

        public AudioFrameMetadata Copy()
        {
            return (AudioFrameMetadata)MemberwiseClone();
        }
    }
}
