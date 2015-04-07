using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;
using Hudl.FFmpeg.Metadata.FFprobe.Options.BaseTypes;

namespace Hudl.FFmpeg.Metadata.FFprobe.Options
{
    internal class PrintFormatJsonOption : IFFprobeOptions
    {
        public string Setting { get { return "-print_format json"; } }
    }
}
