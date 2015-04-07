using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes;
using Hudl.FFmpeg.Metadata.FFprobe.Options.BaseTypes;

namespace Hudl.FFmpeg.Metadata.FFprobe.Options
{
    internal class ShowFormatOption : IFFprobeOptions
    {
        public string Setting { get { return "-show_format"; } }
    }
}
