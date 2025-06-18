using System.Text.Json.Serialization;

namespace Hudl.FFprobe.Metadata.Models;

public class AudioFrameMetadata : BaseFrameMetadata
{
    [JsonPropertyName("sample_fmt")]
    public string SampleFormat { get; set; }

    [JsonPropertyName("channels")]
    public int Channels { get; set; }

    [JsonPropertyName("channel_layout")]
    public string ChannelLayout { get; set; }

    [JsonPropertyName("nb_samples")]
    public int NumberOfSamples { get; set; }

    public AudioFrameMetadata Copy()
    {
        return (AudioFrameMetadata)MemberwiseClone();
    }
}