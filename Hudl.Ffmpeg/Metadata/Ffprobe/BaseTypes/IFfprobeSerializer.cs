using System.Collections.Generic;

namespace Hudl.FFmpeg.Metadata.FFprobe.BaseTypes
{
    internal interface IFFprobeSerializer
    {
        string Tag { get; }

        string Setting { get; }

        List<FFprobeKeyValuePair> Serialize(List<FFprobeKeyValuePair> rawSerializedValues);
    }
}
