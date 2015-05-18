using System.Collections.Generic;
using Hudl.FFmpeg.Filters.Attributes;

namespace Hudl.FFmpeg.Filters.Serialization
{
    internal class FilterSerializerData
    {
        public FilterSerializerData()
        {
            Parameters = new List<FilterSerializerDataParameter>();
        }

        public FilterAttribute Filter { get; set; }

        public List<FilterSerializerDataParameter> Parameters { get; set; } 
    }
}
