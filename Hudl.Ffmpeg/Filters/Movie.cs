using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Movie Video filter declares a filter resource that can be given a specific map. This resource can then be used as an input stream in any subsequent filterchains.
    /// </summary>
    [ForStream(Type=typeof(VideoStream))]
    [Filter(Name = "amovie", MinInputs = 0, MaxInputs = 0)]
    public class Movie : BaseMovie
    {
        public Movie()
        {
        }
        public Movie(IContainer resource)
            : this()
        {
            Resource = resource; 
        }
    }
}
