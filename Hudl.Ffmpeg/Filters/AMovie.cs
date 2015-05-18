using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// AMovie Audio filter declares a filter resource that can be given a specific map. This resource can then be used as an input stream in any subsequent filterchains.
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [Filter(Name = "amovie", MinInputs = 0, MaxInputs = 0)]
    public class AMovie : BaseMovie
    {
        public AMovie()
        {
        }
        public AMovie(IContainer file)
            : this()
        {
            Resource = file;
        }
    }
}
