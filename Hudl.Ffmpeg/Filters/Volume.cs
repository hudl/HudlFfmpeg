using System;
using System.Globalization;
using System.Text;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Volume Filter, overrides the volume of an audio resource by scaling it up and down.
    /// </summary>
    [ForStream(Type=typeof(AudioStream))]
    [Filter(Name = "volume", MinInputs = 1, MaxInputs = 1)]
    public class Volume : IFilter
    {
        public Volume()
        {
        }
        public Volume(decimal scale)
            : this()
        {
            Expression = scale.ToString(CultureInfo.InvariantCulture);
        }

        [FilterParameter(Name = "volume")]
        public string Expression { get; set; }

        [FilterParameter(Name = "replaygain_preamp")]
        [Validate(LogicalOperators.GreaterThan, 0D)]
        public double? ReplayGainPreamp { get; set; }

        [FilterParameter(Name = "precision", Default = VolumePrecisionType.Float, Formatter = typeof(EnumParameterFormatter))]
        public VolumePrecisionType? Precision { get; set; }

        [FilterParameter(Name = "replaygain", Default = VolumeReplayGainType.Drop, Formatter = typeof(EnumParameterFormatter))]
        public VolumeReplayGainType? ReplayGain { get; set; }

        [FilterParameter(Name = "eval", Default = VolumeExpressionEvalType.Once, Formatter = typeof(EnumParameterFormatter))]
        public VolumeExpressionEvalType? Eval { get; set; }
    }
}
