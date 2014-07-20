using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// removes the subtitles stream from the output file
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class RemoveSubtitles : BaseSetting
    {
        private const string SettingType = "-sn";

        public RemoveSubtitles()
            : base(SettingType)
        {
        }
        
        public override string ToString()
        {
            return Type;
        }
    }
}
