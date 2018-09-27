using Newtonsoft.Json;

namespace Hudl.FFprobe.Metadata.Models
{
    [JsonObject]
    public class SubtitleFrameMetadata : BaseFrameMetadata
    {
        public SubtitleFrameMetadata Copy()
        {
            return (SubtitleFrameMetadata)MemberwiseClone();
        }
    }
}
