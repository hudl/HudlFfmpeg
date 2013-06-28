using System;
using System.Drawing;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsApplicationAttribute.SettingsResourceType.Output)]
    public class FrameRate : ISetting
    {
        public FrameRate(double rate)
        {
            if (rate <= 0)
            {
                throw new ArgumentException("Frame rate must be greater than zero.");
            }

            Rate = rate;
        }

        public double Rate { get; set; }

        public string Type { get { return "-r"; } }
        
        public override string ToString()
        {
            if (Rate <= 0)
            {
                throw new ArgumentException("Frame rate must be greater than zero.");
            }

            return string.Concat(Type, " ", Rate);
        }
    }
}
