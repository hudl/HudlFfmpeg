using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Split Filter copys an input video stream into multiple outputs
    /// </summary>
    [ForStream(Type=typeof(VideoStream))]
    public class Split : BaseSplit
    {
        public Split() 
            : base(string.Empty) 
        {
        }
        public Split(int? numberOfStreams)
            : this()
        {
            NumberOfStreams = numberOfStreams;
        }
    }
}
