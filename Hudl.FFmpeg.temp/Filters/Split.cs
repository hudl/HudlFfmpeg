using System;
using System.Linq;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Filters.Contexts;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Split Filter copys an input video stream into multiple outputs
    /// </summary>
    [ForStream(Type=typeof(VideoStream))]
    [Filter(Name = "split", MinInputs = 1, MaxInputs = 1)]
    public class Split : BaseSplit, IFilterProcessor
    {
        public Split()
        {
        }
        public Split(int? numberOfStreams)
            : this()
        {
            NumberOfStreams = numberOfStreams;
        }

        #region IFilterProcessor
        public void Process(FilterProcessorContext context)
        {
            var firstStream = context.Streams.OfType<VideoStream>().FirstOrDefault();
            if (firstStream == null)
            {
                throw new InvalidOperationException("Found a spit filter with zero video streams.");
            }

            for (var i = context.Filterchain.OutputCount; i < NumberOfStreams; i++)
            {
                context.Filterchain.CreateOutput(firstStream.Copy());
            }
        }
        #endregion

    }
}
