using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Hudl.FFmpeg.Filters.Interfaces;

namespace Hudl.FFmpeg.Filters.Serialization
{
    public class FilterSerializer
    {

        public static string Serialize(IFilter filter)
        {

        }

        private static FilterSerializerData GetFilterData(IFilter filter)
        {
            return FilterSerializerAttributeParser.GetFilterSerializerData(filter);
        }
    }
}
