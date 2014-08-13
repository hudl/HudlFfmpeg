using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Movie Video filter declares a filter resource that can be given a specific map. This resource can then be used as an input stream in any subsequent filterchains.
    /// </summary>
    [ForStream(Type=typeof(VideoStream))]
    public class Movie : BaseMovie
    {
        public Movie()
            : base(string.Empty) 
        {
        }
        public Movie(IContainer resource)
            : this()
        {
            Resource = resource; 
        }
    }
}
