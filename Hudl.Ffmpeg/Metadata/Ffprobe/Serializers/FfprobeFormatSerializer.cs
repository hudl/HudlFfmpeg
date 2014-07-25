using System.Collections.Generic;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;

namespace Hudl.FFmpeg.Metadata.FFprobe.Serializers
{
    internal class FFprobeFormatSerializer : IFFprobeSerializer
    {
        public string Tag { get { return "FORMAT"; } }

        public string Setting { get { return "-show_format"; } }
    
        public List<FFprobeKeyValuePair> Serialize(List<FFprobeKeyValuePair> rawSerializedValues)
        {
            var returnValues = new List<FFprobeKeyValuePair>(); 

            //serialize the values that we want to capture
            var encodeApplication = FFprobeGeneralSerializer.SerializeAsFFprobeString(rawSerializedValues, "TAG:encoder");
            if (encodeApplication != null) { returnValues.Add(encodeApplication); }

            return returnValues;
        }
    }
}
