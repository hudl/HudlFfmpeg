using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Designate one or more input streams as a source for the output file.
    /// </summary>
    [ForStream(Type = typeof(IContainer))]
    [Setting(Name = "map", IsMultipleAllowed = true)]
    public class Map : ISetting
    {
        public Map(StreamIdentifier streamId)
        {
            Stream = streamId.Map;
        }
        public Map(string streamId)
        {
            Stream = streamId;
        }

        [SettingValue(Formatter = typeof(MapSettingsFormatter))]
        public string Stream { get; set; }
    }
}
