using Hudl.FFmpeg.Filters.Attributes;

namespace Hudl.FFmpeg.Filters.Serialization
{
    internal class FilterSerializerDataParameter
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsDefault { get; set; }

        public FilterParameterAttribute Parameter { get; set; }
    }
}
