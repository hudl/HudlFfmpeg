using Hudl.FFprobe.Options.BaseTypes;

namespace Hudl.FFprobe.Options
{
    internal class ShowFormatOption : IFFprobeOptions
    {
        public string Setting { get { return "-show_format"; } }
    }
}
