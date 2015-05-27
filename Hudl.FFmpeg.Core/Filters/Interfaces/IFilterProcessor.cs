using Hudl.FFmpeg.Filters.Contexts;

namespace Hudl.FFmpeg.Filters.Interfaces
{
    /// <summary>
    /// The filter processor interface is used when a filter or template requires input video to be in a specified format. The methods allow the programmer to abstract some of that processing by performing it when the filter is applied.
    /// </summary>
    public interface IFilterProcessor
    {
        /// <summary>
        /// Generates a list of Single Input, Single Output prepatory commands, this restraint is enforeced.
        /// </summary>
        void Process(FilterProcessorContext context);
    }
}
