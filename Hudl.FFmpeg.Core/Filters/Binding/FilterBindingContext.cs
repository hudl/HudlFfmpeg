using System;
using Hudl.FFmpeg.Filters.Interfaces;

namespace Hudl.FFmpeg.Filters.Binding
{
    public class FilterBindingContext
    {
        public int? NumberOfStreamsIn { get; internal set; }

        public int? NumberOfStreamsOut { get; internal set; }
    }

    public static class FilterBindings
    {
        public const Type NumberOfStreamsIn = typeof(NumberOfStreamsInBinding);
        public const Type NumberOfStreamsOut = typeof(NumberOfStreamsOutBinding);
    }

    public class NumberOfStreamsInBinding : IFilterParameterBinding
    {
        public string GetValue(FilterBindingContext context)
        {
            return context.NumberOfStreamsIn.ToString();
        }
    }

    public class NumberOfStreamsOutBinding : IFilterParameterBinding
    {
        public string GetValue(FilterBindingContext context)
        {
            return context.NumberOfStreamsOut.ToString();
        }
    }
}
