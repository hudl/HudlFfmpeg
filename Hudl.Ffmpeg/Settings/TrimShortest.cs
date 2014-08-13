using System.Linq;
using System.Collections.Generic;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// The trim shortest setting will trim the output resource to the shortest input resource setting.
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class TrimShortest : BaseSetting
    {
        private const string SettingType = "-shortest";

        public TrimShortest()
            : base(SettingType)
        {
        }

        public override string ToString()
        {
            return Type;
        }

        public MetadataInfo EditInfo(MetadataInfo infoToUpdate, List<MetadataInfo> suppliedInfo)
        {
            return suppliedInfo.OrderBy(r => r.Duration).FirstOrDefault();
        }
    }
}
