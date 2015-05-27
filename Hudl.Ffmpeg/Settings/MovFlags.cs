using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// MovFlags is used to set AVOptions that tell ffmpeg how to fragment the file. 
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "movflags")]
    public class MovFlags : ISetting
    {
        /// <summary>
        /// run a second pass moving the index (moov atom) to the beginning of the file
        /// </summary>
        public const string EnableFastStart = "+faststart"; 

        public MovFlags(string flags)
        {
            Flags = flags;
        }
    
        [SettingValue]
        public string Flags { get; set; }
    }
}
