using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Used to loop input image streams
    /// </summary>
    [ForStream(Type = typeof (VideoStream))]
    [Setting(Name = "loop", IsPreDeclaration = true, ResourceType = SettingsCollectionResourceType.Input)]
    public class LoopInput : ISetting
    {
        [SettingParameter]
        public string Flags
        {
            get { return "1"; }
        }
    }
}