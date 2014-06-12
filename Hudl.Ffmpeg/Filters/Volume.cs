using System;
using System.Globalization;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Volume Filter, overrides the volume of an audio resource by scaling it up and down.
    /// </summary>
    [AppliesToResource(Type=typeof(IAudio))]
    public class Volume : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "volume";

        public Volume()
            : base(FilterType, FilterMaxInputs)
        {
            Scale = 1m;
        }
        public Volume(decimal scale)
            : this()
        {
            Scale = scale;
        }

        public decimal Scale { get; set; }

        public override void Validate()
        {
            if (Scale == 1m)
            {
                throw new InvalidOperationException("Scale has no effect at 100% of the current volume.");
            }
        }

        public override string ToString() 
        {
            return string.Concat(Type, "=volume=", Scale.ToString(CultureInfo.InvariantCulture));
        }
    }
}
