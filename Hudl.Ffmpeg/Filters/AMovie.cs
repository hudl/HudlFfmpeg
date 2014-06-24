using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// AMovie Audio filter declares a filter resource that can be given a specific map. This resource can then be used as an input stream in any subsequent filterchains.
    /// </summary>
    [AppliesToResource(Type=typeof(IAudio))]
    public class AMovie : BaseMovie
    {
        private const string FilterTypePrefix = "a";
        public AMovie()
            : base(FilterTypePrefix)
        {
        }
        public AMovie(IAudio file)
            : this()
        {
            Resource = file;
        }
    }
}
