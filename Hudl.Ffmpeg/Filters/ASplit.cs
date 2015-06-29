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
    /// ASplit Filter copys the input audio stream into multiple outputs
    /// </summary>
    [ForStream(Type=typeof(AudioStream))]
    [Filter(Name = "asplit", MinInputs = 1, MaxInputs = 1)]
    public class ASplit : BaseSplit, IFilterProcessor
    {
        public ASplit()
        {
        }
        public ASplit(int? numberOfStreams)
            : this()
        {
            NumberOfStreams = numberOfStreams;
        }

        #region IFilterProcessor
        public void Process(FilterProcessorContext context)
        {
            var firstStream = context.Streams.OfType<AudioStream>().FirstOrDefault();
            if (firstStream == null)
            {
                throw new InvalidOperationException("Found a aspit filter with zero audio streams.");
            }

            for (var i = context.Filterchain.OutputCount; i < NumberOfStreams; i++)
            {
                context.Filterchain.CreateOutput(firstStream.Copy());
            }
        }
        #endregion
    }
}
