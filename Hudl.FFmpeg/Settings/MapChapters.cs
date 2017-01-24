using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// copy chapters from input file with index input_file_index to the next output file..
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "map_chapters")]
    public class MapChapters : ISetting
    {
        public MapChapters(string mapping)
        {
            Mapping = mapping;
        }

        [SettingParameter]
        public string Mapping { get; set; }
    }
}
