using Hudl.FFprobe.Options.BaseTypes;

namespace Hudl.FFprobe.Options
{
    internal class ShowStreamsOption : IFFprobeOptions
    {
        public string Setting { get { return "-show_streams"; } }
    }
}
