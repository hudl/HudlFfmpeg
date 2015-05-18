using Hudl.FFmpeg.Filters.Binding;

namespace Hudl.FFmpeg.Filters.Interfaces
{
    public interface IFilterParameterBinding 
    {
        string GetValue(FilterBindingContext context);
    }
}
