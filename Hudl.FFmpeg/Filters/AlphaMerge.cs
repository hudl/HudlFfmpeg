using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Adds or replaces the alpha component of the primary input with
    /// the grayscale value of a second input.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name ="alphamerge", MinInputs = 2, MaxInputs = 2)]
    public class AlphaMerge : IFilter
    {
        //This filter accepts no parameters
    }
}
