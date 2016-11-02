using Hudl.FFmpeg.Filters.Contexts;
using Hudl.FFmpeg.Filters.Interfaces;

namespace Hudl.FFmpeg.Filters.Bindings
{
    public class NumberOfStreamsInBinding : IFilterParameterBinding
    {
        public string GetValue(FilterBindingContext context)
        {
            return context.NumberOfStreamsIn.ToString();
        }
    }
}
