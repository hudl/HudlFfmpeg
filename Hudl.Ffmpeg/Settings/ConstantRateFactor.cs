using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Sets the quantizer scale for the encoder on a scale between 0-51: where 0 is lossless and 51 is the worst possible. 18 is visually lossless quality
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class ConstantRateFactor : BaseSetting
    {
        private const string SettingType = "-crf";

        public ConstantRateFactor(int quantizerScale)
            : base(SettingType)
        {
            QuantizerScale = quantizerScale;
        }
    
        public double QuantizerScale { get; set; }

        public override void Validate()
        {
            if (QuantizerScale < 0 || QuantizerScale > 51)
            {
                throw new InvalidOperationException("QuantizerScale size must be between 0 - 51.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", QuantizerScale);
        }
    }
}
