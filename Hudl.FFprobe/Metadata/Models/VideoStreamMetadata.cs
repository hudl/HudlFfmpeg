using Hudl.FFmpeg.DataTypes;
using Hudl.FFprobe.Serialization;
using Hudl.FFprobe.Serialization.Converters;
using Newtonsoft.Json;

namespace Hudl.FFprobe.Metadata.Models
{
    [JsonObject]
    public class VideoStreamMetadata : BaseStreamMetadata
    {
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "coded_width")]
        public int CodedWidth { get; set; }

        [JsonProperty(PropertyName = "coded_height")]
        public int CodedHeight { get; set; }

        [JsonProperty(PropertyName = "has_b_frames")]
        public int HasBFrames { get; set; }

        [JsonProperty(PropertyName = "sample_aspect_ratio")]
        [JsonConverter(typeof(RatioConverter))]
        public Ratio SampleAspectRatio { get; set; }

        [JsonProperty(PropertyName = "display_aspect_ratio")]
        [JsonConverter(typeof(RatioConverter))]
        public Ratio DisplayAspectRatio { get; set; }

        [JsonProperty(PropertyName = "pix_fmt")]
        public string PixelFormat { get; set; }

        [JsonProperty(PropertyName = "level")]
        public int Level { get; set; }

        [JsonProperty(PropertyName = "r_frame_rate")]
        [JsonConverter(typeof(FractionConverter))]
        public Fraction RFrameRate { get; set; }

        [JsonProperty(PropertyName = "avg_frame_rate")]
        [JsonConverter(typeof(FractionConverter))]
        public Fraction AverageFrameRate { get; set; }

        public VideoStreamMetadata Copy()
        {
            return (VideoStreamMetadata) MemberwiseClone(); 
        }
    }
}
