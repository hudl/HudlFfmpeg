using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// set the Dynamic Aspect Ratio for the video resource
    /// </summary>
    [AppliesToResource(Type=typeof(IAudio))]
    public class Volume : IFilter
    {
        public Volume() 
        { 
        }
        public Volume(FfmpegScale scale)
        {
            if (FfmpegScale.IsNullOrZero(scale))
                throw new ArgumentException("Scale cannot be null.", "scale");
            Scale = scale;
        }

        public FfmpegScale Scale { get; set; }

        public string Type { get { return "volume"; } }

        public int MaxInputs { get { return 1; } }

        public override string ToString() 
        {
            if (FfmpegScale.IsNullOrZero(Scale))
                throw new ArgumentException("Scale cannot be null.", "Scale");

            return string.Concat(Type, "=sar=", Scale.ToString());
        }
    }
}
