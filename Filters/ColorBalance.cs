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
    [AppliesToResource(Type=typeof(IVideo))]
    public class ColorBalance : IFilter
    {
        public ColorBalance()
        {
        }
        public ColorBalance(double rs, double gs, double bs, 
                            double rm, double gm, double bm, 
                            double rh, double gh, double bh) : this()
        {
            Shadow.Red = rs;
            Shadow.Blue = bs;
            Shadow.Green = gs;
            Midtone.Red = rm;
            Midtone.Blue = bm;
            Midtone.Green = gm;
            Highlight.Red = rh;
            Highlight.Blue = bh;
            Highlight.Green = gh;
        }

        /// <summary>
        /// property to the RGB shadow color balancing
        /// </summary>
        public new FfmpegScaleRgb Shadow { get; set; }
        
        /// <summary>
        /// property to the RGB midtone color balancing
        /// </summary>
        public new FfmpegScaleRgb Midtone { get; set; }
        
        /// <summary>
        /// property to the RGB highlight color balancing
        /// </summary>
        public new FfmpegScaleRgb Highlight { get; set; }

        public string Type { get { return "colorbalance"; } }

        public int MaxInputs { get { return 2; } }

        public override string ToString() 
        {
            if (Shadow.Red == 0 &&
                Shadow.Green == 0 &&
                Shadow.Blue == 0 &&
                Midtone.Red == 0 &&
                Midtone.Green == 0 &&
                Midtone.Blue == 0 &&
                Highlight.Red == 0 &&
                Highlight.Green == 0 &&
                Highlight.Blue == 0)
                throw new ArgumentException("At least one Color Balance ratio greater or less than 0 is required."); 

            StringBuilder filter = new StringBuilder(100);
            if (Shadow.Red != 0) 
                filter.AppendFormat("{1}rs={0}", 
                    Shadow.Red, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            if (Shadow.Green != 0) 
                filter.AppendFormat("{1}gs={0}", 
                    Shadow.Green, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            if (Shadow.Blue != 0) 
                filter.AppendFormat("{1}bs={0}", 
                    Shadow.Blue, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            if (Midtone.Red != 0) 
                filter.AppendFormat("{1}gm={0}", 
                    Shadow.Green, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            if (Midtone.Blue != 0) 
                filter.AppendFormat("{1}bm={0}", 
                    Shadow.Blue, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            if (Highlight.Red != 0) 
                filter.AppendFormat("{1}rh={0}", 
                    Shadow.Red, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            if (Highlight.Green != 0) 
                filter.AppendFormat("{1}gh={0}", 
                    Shadow.Green, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            if (Highlight.Blue != 0) 
                filter.AppendFormat("{1}bh={0}", 
                    Shadow.Blue, 
                    (filter.Length > 0) ?  ":" : string.Empty);

            return string.Concat(Type, "=", filter.ToString());
        }
    }
}
