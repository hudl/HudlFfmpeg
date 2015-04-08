using System;
using System.Globalization;
using System.Text;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Volume Filter, overrides the volume of an audio resource by scaling it up and down.
    /// </summary>
    [ForStream(Type=typeof(AudioStream))]
    public class Volume : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "volume";

        public Volume()
            : base(FilterType, FilterMaxInputs)
        {
        }
        public Volume(decimal scale)
            : this()
        {
            Expression = scale.ToString(CultureInfo.InvariantCulture);
        }

        public string Expression { get; set; }

        public double? ReplayGainPreamp { get; set; }

        public VolumePrecisionType Precision { get; set; }

        public VolumeReplayGainType ReplayGain { get; set; }

        public VolumeExpressionEvalType Eval { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Expression))
            {
                throw new InvalidOperationException("Expression for a volume command must be specified.");
            }
            if (ReplayGainPreamp.HasValue && ReplayGainPreamp <= 0)
            {
                throw new InvalidOperationException("Replay Gain Preamp must be greater than zero.");
            }
        }

        public override string ToString() 
        {
            var filterParameters = new StringBuilder(100);

            if (!string.IsNullOrWhiteSpace(Expression))
            {
                FilterUtility.ConcatenateParameter(filterParameters, "volume", Expression);
            }
            if (Precision != VolumePrecisionType.Float)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "precision", Formats.EnumValue(Precision));
            }
            if (ReplayGain != VolumeReplayGainType.Drop)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "replaygain", Formats.EnumValue(ReplayGain));
            }
            if (ReplayGainPreamp.HasValue && ReplayGainPreamp.Value > 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "replaygain_preamp", ReplayGainPreamp);
            }
            if (Eval != VolumeExpressionEvalType.Once)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "eval", Formats.EnumValue(Eval));
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }
    }
}
