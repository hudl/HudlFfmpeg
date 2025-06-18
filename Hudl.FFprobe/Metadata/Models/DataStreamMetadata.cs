namespace Hudl.FFprobe.Metadata.Models;

public class DataStreamMetadata : BaseStreamMetadata
{
    public DataStreamMetadata Copy()
    {
        return (DataStreamMetadata)MemberwiseClone();
    }
}