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
    /// set the Sample Aspect Ratio for the video resource
    /// </summary>
    [AppliesToResource(Type=typeof(IVideo))]
    public class SetSar : IFilter
    {
        public SetSar() 
        { 
        }
        public SetSar(FfmpegRatio ratio)
        {
            if (ratio == null)
                throw new ArgumentException("Ratio cannot be null.", "ratio");
            Ratio = ratio;
        }

        public FfmpegRatio Ratio { get; set; } 
               
        public string Type { get { return "setsar"; } }

        public int MaxInputs { get { return 1; } }

        public override string ToString() 
        {
            if (Ratio == null)
                throw new ArgumentException("Ratio cannot be null.", "Ratio");

            return string.Concat(Type, "=sar=", Ratio);
        }
    }
}
