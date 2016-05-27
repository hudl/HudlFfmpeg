using Newtonsoft.Json;

namespace Hudl.FFprobe.Metadata.Models
{
    [JsonObject]
    public class DataStreamMetadata : BaseStreamMetadata
    {
        public DataStreamMetadata Copy()
        {
            return (DataStreamMetadata)MemberwiseClone();
        }
    }
}
