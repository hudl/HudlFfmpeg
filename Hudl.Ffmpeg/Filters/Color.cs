using System;
using System.Collections.Generic;
using System.Drawing;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.DataTypes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Metadata.Models;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Validators;
using Hudl.FFprobe.Metadata.Models;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Filter that returns unprocessed video frames. It is mainly useful to be employed in 
    /// analysis / debugging tools, or as the source for filters which ignore the input data.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name = "color", MinInputs = 0, MaxInputs = 0)]
    public class Color : IFilter, IMetadataManipulation 
    {
        [FilterParameter(Name = "c")]
        public string ColorName { get; set; }

        [FilterParameter(Name = "s", Formatter = typeof(SizeFormatter))]
        public Size? Size { get; set; }

        [FilterParameter(Name = "r", Default = 25)]
        public int? FrameRate { get; set; }

        [FilterParameter(Name = "d", Formatter = typeof(TimeSpanFormatter))]
        [Validate(typeof(TimeSpanGreterThanZeroValidator))]
        public TimeSpan? Duration { get; set; }

        [FilterParameter(Name = "sar", Formatter = typeof(RatioFractionalStringFormatter))]
        public Ratio SampleAspectRatio { get; set; }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            var emptyMetadataInfo = MetadataInfo.Create();

            emptyMetadataInfo.VideoMetadata = new VideoStreamMetadata
                {
                    Width = Size.HasValue ? Size.Value.Width : 320,
                    Height = Size.HasValue ? Size.Value.Height : 240,
                    AverageFrameRate = Fraction.Create(FrameRate ?? 25, 1),
                    RFrameRate = Fraction.Create(FrameRate ?? 25, 1),
                    Duration = Duration ?? TimeSpan.MaxValue,
                    DurationTs = long.MaxValue,
                };

            return MetadataInfoTreeContainer.Create(VideoStream.Create(emptyMetadataInfo));
        }

    }
}
