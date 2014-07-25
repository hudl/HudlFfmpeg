using System.Collections.Generic;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;

namespace Hudl.FFmpeg.Metadata.FFprobe.Serializers
{
    internal class FFprobeStreamSerializer : IFFprobeSerializer
    {
        public string Tag { get { return "STREAM"; } }

        public string Setting { get { return "-show_streams"; } }
    
        public List<FFprobeKeyValuePair> Serialize(List<FFprobeKeyValuePair> rawSerializedValues)
        {
            var returnValues = new List<FFprobeKeyValuePair>(); 

            //serialize the values that we want to capture
            var codecType = FFprobeGeneralSerializer.SerializeAsFFprobeString(rawSerializedValues, "codec_type");
            if (codecType != null) { returnValues.Add(codecType); }

            var timebase = FFprobeGeneralSerializer.SerializeAsFFprobeFraction(rawSerializedValues, "time_base");
            if (timebase != null) { returnValues.Add(timebase); }

            var frameRate = FFprobeGeneralSerializer.SerializeAsFFprobeFraction(rawSerializedValues, "r_frame_rate");
            if (frameRate != null) { returnValues.Add(frameRate); }

            var avgFrameRate = FFprobeGeneralSerializer.SerializeAsFFprobeFraction(rawSerializedValues, "avg_frame_rate");
            if (avgFrameRate != null) { returnValues.Add(avgFrameRate); }

            var duration = FFprobeGeneralSerializer.SerializeAsFFprobeDouble(rawSerializedValues, "duration");
            if (duration != null) { returnValues.Add(duration); }

            var bitrate = FFprobeGeneralSerializer.SerializeAsFFprobeLong(rawSerializedValues, "bit_rate");
            if (bitrate != null) { returnValues.Add(bitrate); }

            var width = FFprobeGeneralSerializer.SerializeAsFFprobeInt(rawSerializedValues, "width");
            if (width != null) { returnValues.Add(width); }

            var height = FFprobeGeneralSerializer.SerializeAsFFprobeInt(rawSerializedValues, "height");
            if (height != null) { returnValues.Add(height); }

            return returnValues;
        }
    }
}
