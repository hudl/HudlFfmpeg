using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Split Filter copys an input video stream into multiple outputs
    /// </summary>
    [AppliesToResource(Type=typeof(IVideo))]
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
