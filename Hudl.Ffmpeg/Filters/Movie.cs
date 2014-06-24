using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Movie Video filter declares a filter resource that can be given a specific map. This resource can then be used as an input stream in any subsequent filterchains.
    /// </summary>
    [AppliesToResource(Type=typeof(IVideo))]
    [AppliesToResource(Type=typeof(IImage))]
    public class Movie : BaseMovie
    {
        public Movie()
            : base(string.Empty) 
        {
        }
        public Movie(IVideo resource)
            : this()
        {
            Resource = resource; 
        }
    }
}
