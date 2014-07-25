using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// ASplit Filter copys the input audio stream into multiple outputs
    /// </summary>
    [ForStream(Type=typeof(AudioStream))]
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
