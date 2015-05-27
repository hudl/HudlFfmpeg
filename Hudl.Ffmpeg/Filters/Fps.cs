using System;
using System.Text;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Filters.Formatters;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// fps filter will set a frames per second on the stream, can be used on images as well
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name = "fps", MinInputs = 1, MaxInputs = 1)]
    public class Fps : IFilter
    {
        public Fps()
        {
        }
        public Fps(double frameRate)
        {
            FrameRate = frameRate;
        }

        [FilterParameter(Name = "fps")]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public double? FrameRate { get; set; }

        [FilterParameter(Name = "start_time")]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public double? StartTime { get; set; }

        [FilterParameter(Name = "round", Default = FpsRoundType.Near, Formatter = typeof(EnumParameterFormatter))]
        public FpsRoundType? Round { get; set; }
    }
}
