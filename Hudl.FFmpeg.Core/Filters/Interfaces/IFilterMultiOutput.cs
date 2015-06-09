using Hudl.FFmpeg.Filters.Contexts;

namespace Hudl.FFmpeg.Filters.Interfaces
{
    public interface IFilterMultiOutput
    {
        /// <summary>
        /// Returns a count for the amount of output streams the filter will result in
        /// </summary>
        int OutputCount(FilterMultiOutputContext context);
    }
}
