using Hudl.FFmpeg.Filters.Contexts;
using Hudl.FFmpeg.Filters.Interfaces;

namespace Hudl.FFmpeg.Filters.Serialization
{
    public class FilterSerializer
    {
        public static string Serialize(IFilter filter)
        {
            return Serialize(filter, FilterBindingContext.Empty());
        }

        public static string Serialize(IFilter filter, FilterBindingContext context)
        {
            var filterData = GetFilterData(filter, context);
            var filterSerializer = new FilterSerializerWriter(filterData);

            return filterSerializer.Write();
        }

        private static FilterSerializerData GetFilterData(IFilter filter, FilterBindingContext context)
        {
            return FilterSerializerAttributeParser.GetFilterSerializerData(filter, context);
        }
    }
}
