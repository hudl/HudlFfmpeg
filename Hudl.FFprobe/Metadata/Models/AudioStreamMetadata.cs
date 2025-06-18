using System.Text.Json.Serialization;

namespace Hudl.FFprobe.Metadata.Models;

public class AudioStreamMetadata : BaseStreamMetadata
{
    [JsonPropertyName("sample_fmt")]
    public string SampleFormat { get; set; }

    [JsonPropertyName("sample_rate")]
    public int SampleRate { get; set; }

    [JsonPropertyName("channels")]
    public int Channels { get; set; }

    [JsonPropertyName("channel_layout")]
    public string ChannelLayout { get; set; }

    [JsonPropertyName("bits_per_sample")]
    public int BitsPerSample { get; set; }

    public AudioStreamMetadata Copy()
    {
        return (AudioStreamMetadata)MemberwiseClone();
    }
}