using System.Collections.Generic;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;

namespace Hudl.FFmpeg.Metadata.FFprobe
{
    internal class FFprobeSerializerResultItem 
    {
        private FFprobeSerializerResultItem(string type, List<FFprobeKeyValuePair> valuesPairs)
        {
            Type = type;
            ValuePairs = valuesPairs;
        }

        public string Type { get; set; }

        public List<FFprobeKeyValuePair> ValuePairs { get; set; }

        public static FFprobeSerializerResultItem Create(string type, List<FFprobeKeyValuePair> valuePairs)
        {
            return new FFprobeSerializerResultItem(type, valuePairs);
        }
    }
}
