using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// ASplit Filter copys the input audio stream into multiple outputs
    /// </summary>
    [ForStream(Type=typeof(AudioStream))]
    [Filter(Name = "asplit", MinInputs = 1, MaxInputs = 1)]
    public class ASplit : BaseSplit
    {
        public ASplit()
        {
        }
        public ASplit(int? numberOfStreams)
            : this()
        {
            NumberOfStreams = numberOfStreams;
        }
    }
}
