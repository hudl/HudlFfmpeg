using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Reverses a video clip. Note that it will buffer the entire clip,
    /// so it's recommented to use trimming.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name ="reverse", MinInputs = 1, MaxInputs = 1)]
    public class Reverse : IFilter
    {
        //This filter accepts no parameters
    }
}
