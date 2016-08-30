using Hudl.FFmpeg.DataTypes;
using Hudl.FFprobe.Serialization;
using Hudl.FFprobe.Serialization.Converters;
using Newtonsoft.Json;

namespace Hudl.FFprobe.Metadata.Models
{
    [JsonObject]
    public class VideoFrameMetadata : BaseFrameMetadata
    {
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "sample_aspect_ratio")]
        [JsonConverter(typeof(RatioConverter))]
        public Ratio SampleAspectRatio { get; set; }

        [JsonProperty(PropertyName = "pix_fmt")]
        public string PixelFormat { get; set; }

        [JsonProperty(PropertyName = "pict_type")]
        public string PictureType { get; set; }

        public VideoFrameMetadata Copy()
        {
            return (VideoFrameMetadata) MemberwiseClone(); 
        }
    }
}
