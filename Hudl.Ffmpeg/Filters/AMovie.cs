using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// AMovie Audio filter declares a filter resource that can be given a specific map. This resource can then be used as an input stream in any subsequent filterchains.
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    public class AMovie : BaseMovie
    {
        private const string FilterTypePrefix = "a";
        public AMovie()
            : base(FilterTypePrefix)
        {
        }
        public AMovie(IContainer file)
            : this()
        {
            Resource = file;
        }
    }
}
