using System;
using System.Text;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.DataTypes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// ColorBalance filter adjusts the color balance on the output video by intensifying the colors in each frame of video.
    /// </summary>
    [ForStream(Type=typeof(VideoStream))]
    [Filter(Name="colorbalance", MinInputs = 1, MaxInputs = 2)]
    public class ColorBalance : IFilter
    {
        public ColorBalance()
        {
            _shadowsRgb = new DecimalScaleRgb();
            _midtonesRgb = new DecimalScaleRgb();
            _highlightsRgb = new DecimalScaleRgb();
        }
        public ColorBalance(DecimalScaleRgb shadows, DecimalScaleRgb midtones, DecimalScaleRgb highlights)
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

            _shadowsRgb = shadows;
            _midtonesRgb = midtones;
            _highlightsRgb = highlights; 
        }

        private readonly DecimalScaleRgb _shadowsRgb;
        private readonly DecimalScaleRgb _midtonesRgb;
        private readonly DecimalScaleRgb _highlightsRgb;

        [FilterParameter(Name="rs")]
        public decimal ShadowRed { get { return _shadowsRgb.Red.Value; } set { _shadowsRgb.Red = value; } }

        [FilterParameter(Name = "gs")]
        public decimal ShadowGreen { get { return _shadowsRgb.Green.Value; } set { _shadowsRgb.Green = value; } }

        [FilterParameter(Name = "bs")]
        public decimal ShadowBlue { get { return _shadowsRgb.Blue.Value; } set { _shadowsRgb.Blue = value; } }

        [FilterParameter(Name = "rm")]
        public decimal MidtonesRed { get { return _midtonesRgb.Red.Value; } set { _midtonesRgb.Red = value; } }

        [FilterParameter(Name = "gm")]
        public decimal MidtonesGreen { get { return _midtonesRgb.Green.Value; } set { _midtonesRgb.Green = value; } }

        [FilterParameter(Name = "bm")]
        public decimal MidtonesBlue { get { return _midtonesRgb.Blue.Value; } set { _midtonesRgb.Blue = value; } }

        [FilterParameter(Name = "rh")]
        public decimal HighlightsRed { get { return _highlightsRgb.Red.Value; } set { _highlightsRgb.Red = value; } }

        [FilterParameter(Name = "gh")]
        public decimal HighlightsGreen { get { return _highlightsRgb.Green.Value; } set { _highlightsRgb.Green = value; } }

        [FilterParameter(Name = "bh")]
        public decimal HighlightsBlue { get { return _highlightsRgb.Blue.Value; } set { _highlightsRgb.Blue = value; } }
    }
}
