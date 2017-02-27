using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name ="reverse", MinInputs = 1, MaxInputs = 1)]
    public class Reverse : IFilter
    {
        //This filter accepts no parameters
    }
}
