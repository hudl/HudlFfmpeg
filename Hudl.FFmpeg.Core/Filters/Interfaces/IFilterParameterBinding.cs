using Hudl.FFmpeg.Filters.Contexts;

namespace Hudl.FFmpeg.Filters.Interfaces
{
    public interface IFilterParameterBinding 
    {
        string GetValue(FilterBindingContext context);
    }
}
