using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IAudio))]
    [AppliesToResource(Type = typeof(IVideo))]
    [AppliesToResource(Type = typeof(IImage))]
    [SettingsApplication(PreDeclaration = true, MultipleAllowed = false, ResourceType = SettingsCollectionResourceType.Output)]
    public class MaxRate : BaseSetting
    {
       private const string SettingType = "-maxrate";

       public MaxRate(int rate)
            : base(SettingType) 
        {
            Rate = rate;
        }

        public int Rate { get; set; }

        public override string ToString()
        {
            if (Rate <= 0)
            {
                throw new InvalidOperationException("Bit Rate must be greater than zero.");
            }

            return string.Concat(Type, " ", Rate, "k");
        }
    }
}
