using System.Text.Json.Serialization;
using Hudl.FFmpeg.DataTypes;
using Hudl.FFprobe.Serialization.Converters;

namespace Hudl.FFprobe.Metadata.Models;

public class VideoFrameMetadata : BaseFrameMetadata
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("sample_aspect_ratio")]
    [JsonConverter(typeof(RatioConverter))]
    public Ratio SampleAspectRatio { get; set; }

    [JsonPropertyName("pix_fmt")]
    public string PixelFormat { get; set; }

    [JsonPropertyName("pict_type")]
    public string PictureType { get; set; }

    public VideoFrameMetadata Copy()
    {
        return (VideoFrameMetadata) MemberwiseClone(); 
    }
}