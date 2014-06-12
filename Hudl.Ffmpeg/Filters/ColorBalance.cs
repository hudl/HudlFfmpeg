using System;
using System.Text;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// ColorBalance filter adjusts the color balance on the output video by intensifying the colors in each frame.
    /// </summary>
    [AppliesToResource(Type=typeof(IVideo))]
    public class ColorBalance : BaseFilter
    {
        private const int FilterMaxInputs = 2;
        private const string FilterType = "colorbalance";

        public ColorBalance()
            : base(FilterType, FilterMaxInputs)
        {
            Shadow = new FfmpegScaleRgb();
            Midtone = new FfmpegScaleRgb();
            Highlight = new FfmpegScaleRgb();
        }
        public ColorBalance(FfmpegScaleRgb shadows, FfmpegScaleRgb midtones, FfmpegScaleRgb highlights)
            : base(FilterType, FilterMaxInputs)
        {
            if (shadows == null)
            {
                throw new ArgumentNullException("shadows");
            }
            if (midtones == null)
            {
                throw new ArgumentNullException("midtones");
            }
            if (highlights == null)
            {
                throw new ArgumentNullException("highlights");
            }

            Shadow = shadows;
            Midtone = midtones;
            Highlight = highlights; 
        }

        /// <summary>
        /// property to the RGB shadow color balancing
        /// </summary>
        public FfmpegScaleRgb Shadow { get; set; }
        
        /// <summary>
        /// property to the RGB midtone color balancing
        /// </summary>
        public FfmpegScaleRgb Midtone { get; set; }
        
        /// <summary>
        /// property to the RGB highlight color balancing
        /// </summary>
        public FfmpegScaleRgb Highlight { get; set; }

        public override void Validate()
        {
            if (Shadow.Red.Value == 0 &&
                Shadow.Green.Value == 0 &&
                Shadow.Blue.Value == 0 &&
                Midtone.Red.Value == 0 &&
                Midtone.Green.Value == 0 &&
                Midtone.Blue.Value == 0 &&
                Highlight.Red.Value == 0 &&
                Highlight.Green.Value == 0 &&
                Highlight.Blue.Value == 0)
            {
                throw new InvalidOperationException("At least one Color Balance ratio greater or less than 0 is required.");
            }
        }

        public override string ToString() 
        {
            var filter = new StringBuilder(100);
            if (Shadow.Red.Value != 0)
            {
                filter.AppendFormat("{1}rs={0}", 
                    Shadow.Red, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            }
            if (Shadow.Green.Value != 0) 
            {
                filter.AppendFormat("{1}gs={0}", 
                    Shadow.Green, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            }
            if (Shadow.Blue.Value != 0) 
            {
                filter.AppendFormat("{1}bs={0}", 
                    Shadow.Blue, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            }
            if (Midtone.Red.Value != 0) 
            {
                filter.AppendFormat("{1}rm={0}",
                    Midtone.Red, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            }
            if (Midtone.Blue.Value != 0) 
            {
                filter.AppendFormat("{1}bm={0}",
                    Midtone.Blue, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            }
            if (Midtone.Green.Value != 0)
            {
                filter.AppendFormat("{1}gm={0}",
                    Midtone.Green,
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Highlight.Red.Value != 0) 
            {
                filter.AppendFormat("{1}rh={0}",
                    Highlight.Red, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            }
            if (Highlight.Green.Value != 0) 
            {
                filter.AppendFormat("{1}gh={0}",
                    Highlight.Green, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            }
            if (Highlight.Blue.Value != 0)
            {
                filter.AppendFormat("{1}bh={0}",
                    Highlight.Blue, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            }

            return string.Concat(Type, "=", filter.ToString());
        }
    }
}
