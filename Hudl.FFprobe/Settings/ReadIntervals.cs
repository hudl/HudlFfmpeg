using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFprobe.Settings
{
    [Setting(Name = "print_format")]
    internal class PrintFormat : ISetting
    {
        public const string JsonFormat = "json";

        public PrintFormat(string format)
        {
            Format = format;
        }

        [SettingParameter]
        public string Format { get; set; }
    }
}
