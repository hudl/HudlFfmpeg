using System;
using System.Text;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// ColorBalance filter adjusts the color balance on the output video by intensifying the colors in each frame of video.
    /// </summary>
    [ForStream(Type=typeof(VideoStream))]
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
            var filterParameters = new StringBuilder(100);

            if (Shadow.Red.Value != 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "rs", Shadow.Red);
            }
            if (Shadow.Green.Value != 0) 
            {
                FilterUtility.ConcatenateParameter(filterParameters, "gs", Shadow.Green);
            }
            if (Shadow.Blue.Value != 0) 
            {
                FilterUtility.ConcatenateParameter(filterParameters, "bs", Shadow.Blue);
            }
            if (Midtone.Red.Value != 0) 
            {
                FilterUtility.ConcatenateParameter(filterParameters, "rm", Midtone.Red);
            }
            if (Midtone.Green.Value != 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "gm", Midtone.Green);
            }
            if (Midtone.Blue.Value != 0) 
            {
                FilterUtility.ConcatenateParameter(filterParameters, "bm", Midtone.Blue);
            }
            if (Highlight.Red.Value != 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "rh", Highlight.Red);
            }
            if (Highlight.Green.Value != 0) 
            {
                FilterUtility.ConcatenateParameter(filterParameters, "gh", Highlight.Green);
            }
            if (Highlight.Blue.Value != 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "bh", Highlight.Blue);
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }
    }
}
