using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// ASplit Filter copys the input stream into multiple outputs
    /// </summary>
    [AppliesToResource(Type=typeof(IAudio))]
    public class ASplit : BaseSplitFilter
    {
        private const string FilterType = "asplit";
        private const int DefaultVideoOut = 2;
        private const int FilterMaxInputs = 1;

        public ASplit() 
            : base(FilterType, FilterMaxInputs, DefaultVideoOut)
        {
        }
        public ASplit(int numberOfStreams)
            : base(FilterType, FilterMaxInputs, numberOfStreams)
        {
        }
    }
}
