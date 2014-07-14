using System.Collections.Generic;
using Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes;

namespace Hudl.Ffmpeg.Metadata.Ffprobe.Serializers
{
    internal class FfprobeStreamSerializer : IFfprobeSerializer
    {
        public string Tag { get { return "STREAM"; } }

        public string Setting { get { return "-show_streams"; } }
    
        public List<FfprobeKeyValuePair> Serialize(List<FfprobeKeyValuePair> rawSerializedValues)
        {
            var returnValues = new List<FfprobeKeyValuePair>(); 

            //serialize the values that we want to capture
            var codecType = FfprobeGeneralSerializer.SerializeAsFfprobeString(rawSerializedValues, "codec_type");
            if (codecType != null) { returnValues.Add(codecType); }

            var timebase = FfprobeGeneralSerializer.SerializeAsFfprobeFraction(rawSerializedValues, "time_base");
            if (timebase != null) { returnValues.Add(timebase); }

            var frameRate = FfprobeGeneralSerializer.SerializeAsFfprobeFraction(rawSerializedValues, "r_frame_rate");
            if (frameRate != null) { returnValues.Add(frameRate); }

            var avgFrameRate = FfprobeGeneralSerializer.SerializeAsFfprobeFraction(rawSerializedValues, "avg_frame_rate");
            if (avgFrameRate != null) { returnValues.Add(avgFrameRate); }

            var duration = FfprobeGeneralSerializer.SerializeAsFfprobeDouble(rawSerializedValues, "duration");
            if (duration != null) { returnValues.Add(duration); }

            var bitrate = FfprobeGeneralSerializer.SerializeAsFfprobeLong(rawSerializedValues, "bit_rate");
            if (bitrate != null) { returnValues.Add(bitrate); }

            var width = FfprobeGeneralSerializer.SerializeAsFfprobeInt(rawSerializedValues, "width");
            if (width != null) { returnValues.Add(width); }

            var height = FfprobeGeneralSerializer.SerializeAsFfprobeInt(rawSerializedValues, "height");
            if (height != null) { returnValues.Add(height); }

            return returnValues;
        }
    }
}
