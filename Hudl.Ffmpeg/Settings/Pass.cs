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
    public class Pass : BaseSetting
    {
        private const string SettingType = "-pass";

        public Pass(int number)
            : base(SettingType)
        {
            Number = number; 
        }
    
        public int Number { get; set; }

        public override string ToString()
        {
            if (Number != 1 && Number != 2)
            {
                throw new InvalidOperationException("Pass must be 1 or 2.");
            }

            return string.Concat(Type, " ", Number);
        }
    }
}
