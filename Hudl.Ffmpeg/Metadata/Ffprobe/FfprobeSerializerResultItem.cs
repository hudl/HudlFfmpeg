using System.Collections.Generic;
using Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes;

namespace Hudl.Ffmpeg.Metadata.Ffprobe
{
    internal class FfprobeSerializerResultItem 
    {
        private FfprobeSerializerResultItem(string type, List<FfprobeKeyValuePair> valuesPairs)
        {
            Type = type;
            ValuePairs = valuesPairs;
        }

        public string Type { get; set; }

        public List<FfprobeKeyValuePair> ValuePairs { get; set; }

        public static FfprobeSerializerResultItem Create(string type, List<FfprobeKeyValuePair> valuePairs)
        {
            return new FfprobeSerializerResultItem(type, valuePairs);
        }
    }
}
