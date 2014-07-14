using System.Collections.Generic;
using Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes;

namespace Hudl.Ffmpeg.Metadata.Ffprobe.Serializers
{
    internal class FfprobeFormatSerializer : IFfprobeSerializer
    {
        public string Tag { get { return "FORMAT"; } }

        public string Setting { get { return "-show_format"; } }
    
        public List<FfprobeKeyValuePair> Serialize(List<FfprobeKeyValuePair> rawSerializedValues)
        {
            var returnValues = new List<FfprobeKeyValuePair>(); 

            //serialize the values that we want to capture
            var encodeApplication = FfprobeGeneralSerializer.SerializeAsFfprobeString(rawSerializedValues, "TAG:encoder");
            if (encodeApplication != null) { returnValues.Add(encodeApplication); }

            return returnValues;
        }
    }
}
