using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// when specified will overwrite an existing output file. 
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class OverwriteOutput : BaseSetting
    {
        private const string SettingType = "-y";

        public OverwriteOutput()
            : base(SettingType)
        {
        }

        public override string ToString()
        {
            return Type;
        }
    }
}
