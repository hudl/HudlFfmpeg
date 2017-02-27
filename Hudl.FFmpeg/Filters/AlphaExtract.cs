using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Extracts the alpha component from the input as a grayscale video.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name ="alphaextract", MinInputs = 1, MaxInputs = 1)]
    public class AlphaExtract : IFilter
    {
        //This filter accepts no parameters
    }
}
