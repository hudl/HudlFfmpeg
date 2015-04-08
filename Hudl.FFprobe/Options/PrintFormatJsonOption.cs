using Hudl.FFprobe.Options.BaseTypes;

namespace Hudl.FFprobe.Options
{
    internal class PrintFormatJsonOption : IFFprobeOptions
    {
        public string Setting { get { return "-print_format json"; } }
    }
}
