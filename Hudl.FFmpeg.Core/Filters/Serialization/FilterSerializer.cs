using Hudl.FFmpeg.Filters.Interfaces;

namespace Hudl.FFmpeg.Filters.Serialization
{
    public class FilterSerializer
    {
        public static string Serialize(IFilter filter)
        {
            var filterData = GetFilterData(filter);
            var filterSerializer = new FilterSerializerWriter(filterData);

            return filterSerializer.Write();
        }

        private static FilterSerializerData GetFilterData(IFilter filter)
        {
            return FilterSerializerAttributeParser.GetFilterSerializerData(filter);
        }
    }
}
