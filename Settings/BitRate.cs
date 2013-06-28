using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsApplicationAttribute.SettingsResourceType.Output)]
    public class BitRate : ISetting
    {
        public BitRate(int rate)
        {
            Rate = rate;
        }

        public int Rate { get; set; }

        public string Type { get { return "-bt"; } }
        
        public override string ToString()
        {
            if (Rate <= 0)
            {
                throw new ArgumentException("Bit Rate must be greater than zero.");
            }

            return string.Concat(Type, " ", Rate, "k");
        }
    }
}
