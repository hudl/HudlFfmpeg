using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Split Filter copys the input stream into multiple outputs
    /// </summary>
    [AppliesToResource(Type=typeof(IVideo))]
    public class Split : BaseSplitFilter
    {
        private const string FilterType = "split";
        private const int DefaultVideoOut = 2;
        private const int FilterMaxInputs = 1;

        public Split() 
            : base(FilterType, FilterMaxInputs, DefaultVideoOut)
        {
        }
        public Split(int numberOfStreams)
            : base(FilterType, FilterMaxInputs, numberOfStreams)
        {
        }
    }
}
