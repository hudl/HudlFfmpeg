using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [ForStream(Type = typeof(AudioStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class SampleRate : BaseSetting
    {
        private const string SettingType = "-ar";

        public SampleRate(double rate)
            : base(SettingType)
        {
            if (rate <= 0)
            {
                throw new ArgumentException("Sample rate must be greater than zero.");
            }

            Rate = rate;
        }

        public double Rate { get; set; }

        public override void Validate()
        {
            if (Rate <= 0)
            {
                throw new InvalidOperationException("Sample rate must be greater than zero.");
            }
        }

        public override string ToString()
        {
           
            return string.Concat(Type, " ", Rate);
        }
    }
}
