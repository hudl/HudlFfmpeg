using System.Text.Json.Serialization;
using Hudl.FFmpeg.DataTypes;
using Hudl.FFprobe.Serialization.Converters;

namespace Hudl.FFprobe.Metadata.Models;

public class VideoStreamMetadata : BaseStreamMetadata
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("coded_width")]
    public int CodedWidth { get; set; }

    [JsonPropertyName("coded_height")]
    public int CodedHeight { get; set; }

    [JsonPropertyName("has_b_frames")]
    public int HasBFrames { get; set; }

    [JsonPropertyName("sample_aspect_ratio")]
    [JsonConverter(typeof(RatioConverter))]
    public Ratio SampleAspectRatio { get; set; }

    [JsonPropertyName("display_aspect_ratio")]
    [JsonConverter(typeof(RatioConverter))]
    public Ratio DisplayAspectRatio { get; set; }

    [JsonPropertyName("pix_fmt")]
    public string PixelFormat { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("r_frame_rate")]
    [JsonConverter(typeof(FractionConverter))]
    public Fraction RFrameRate { get; set; }

    [JsonPropertyName("avg_frame_rate")]
    [JsonConverter(typeof(FractionConverter))]
    public Fraction AverageFrameRate { get; set; }

    public VideoStreamMetadata Copy()
    {
        return (VideoStreamMetadata) MemberwiseClone(); 
    }
}