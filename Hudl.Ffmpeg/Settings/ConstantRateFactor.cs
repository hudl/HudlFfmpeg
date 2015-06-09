using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Sets the quantizer scale for the encoder on a scale between 0-51: where 0 is lossless and 51 is the worst possible. 18 is visually lossless quality
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "crf")]
    public class ConstantRateFactor : ISetting
    {
        public ConstantRateFactor(int quantizerScale)
        {
            QuantizerScale = quantizerScale;
        }
    
        [SettingParameter]
        [Validate(LogicalOperators.GreaterThanOrEqual, 0)]
        [Validate(LogicalOperators.LesserThanOrEqual, 51)]
        public double QuantizerScale { get; set; }
    }
}
