using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// ASplit Filter copys the input audio stream into multiple outputs
    /// </summary>
    [AppliesToResource(Type=typeof(IAudio))]
    public class ASplit : BaseSplit
    {
        private const string FilterTypePrefix = "a";

        public ASplit()
            : base(FilterTypePrefix)
        {
        }
        public ASplit(int? numberOfStreams)
            : this()
        {
            NumberOfStreams = numberOfStreams;
        }
    }
}
