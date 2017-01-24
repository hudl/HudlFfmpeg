using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// set metadata information of the next output file from infile.
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "map_metadata")]
    public class MapMetadata : ISetting
    {
        public MapMetadata(string mapping)
        {
            Mapping = mapping;
        }

        [SettingParameter]
        public string Mapping { get; set; }
    }
}
