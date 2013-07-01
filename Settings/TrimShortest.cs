using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceTypes.Output)]
    public class TrimShortest : BaseSetting
    {
        private const string SettingType = "-shortest";

        public TrimShortest()
            : base(SettingType)
        {
        }

        public override TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources)
        {
            return resources.Min(r => r.Resource.Length);
        }

        public override string ToString()
        {
            return Type;
        }
    }
}
