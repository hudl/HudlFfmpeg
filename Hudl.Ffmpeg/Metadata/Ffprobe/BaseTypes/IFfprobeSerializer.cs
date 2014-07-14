using System.Collections.Generic;

namespace Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes
{
    internal interface IFfprobeSerializer
    {
        string Tag { get; }

        string Setting { get; }

        List<FfprobeKeyValuePair> Serialize(List<FfprobeKeyValuePair> rawSerializedValues);
    }
}
