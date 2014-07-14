using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Metadata;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// The trim shortest setting will trim the output resource to the shortest input resource setting.
    /// </summary>
    [AppliesToResource(Type = typeof(IAudio))]
    [AppliesToResource(Type = typeof(IVideo))]
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
