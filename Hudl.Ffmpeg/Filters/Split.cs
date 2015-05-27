using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Split Filter copys an input video stream into multiple outputs
    /// </summary>
    [ForStream(Type=typeof(VideoStream))]
    [Filter(Name = "split", MinInputs = 1, MaxInputs = 1)]
    public class Split : BaseSplit
    {
        public Split()
        {
        }
        public Split(int? numberOfStreams)
            : this()
        {
            NumberOfStreams = numberOfStreams;
        }
    }
}
