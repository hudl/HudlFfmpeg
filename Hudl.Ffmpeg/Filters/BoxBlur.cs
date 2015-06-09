using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Video filter that applies a boxblur algorithm to the input video.
    /// </summary>
    [ForStream(Type=typeof(VideoStream))]
    [Filter(Name = "boxblur", MinInputs = 1, MaxInputs = 1)]
    public class BoxBlur : IFilter
    {
        public BoxBlur()
        {
        }
        public BoxBlur(string expression)
            : this()
        {
            Expression = expression; 
        }

        [FilterParameter]
        public string Expression { get; set; }
    }
}
