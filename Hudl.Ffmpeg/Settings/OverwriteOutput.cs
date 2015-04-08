using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
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
