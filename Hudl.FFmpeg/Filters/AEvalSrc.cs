using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Attributes;
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
    /// Filter that return unprocessed audio frames. It is mainly useful as a template and to be employed in 
    /// analysis / debugging tools, or as the source for filters which ignore the input data (for example 
    /// the sox synth filter).
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [Filter(Name = "aevalsrc", MinInputs = 0, MaxInputs = 0)]
    public class AEvalSrc : IFilter, IMetadataManipulation 
    {
        [FilterParameter(Name = "exprs")]
        public string Expression { get; set; }

        [FilterParameter(Name = "c")]
        public string ChannelLayout { get; set; }

        [FilterParameter(Name = "d", Formatter = typeof(TimeSpanFormatter))]
        [Validate(typeof(TimeSpanGreterThanZeroValidator))]
        public TimeSpan? Duration { get; set; }

        [FilterParameter(Name = "s", Default = 44100)]
        public int? SampleRate { get; set; }

        [FilterParameter(Name = "n", Default = 1024)]
        public long? NumberOfSamples { get; set; }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            var emptyMetadataInfo = MetadataInfo.Create();

            emptyMetadataInfo.AudioMetadata = new AudioStreamMetadata
                {
                    SampleRate = SampleRate ?? 44100,
                    Duration = Duration ?? TimeSpan.MaxValue,
                    DurationTs = long.MaxValue,
                };

            return MetadataInfoTreeContainer.Create(AudioStream.Create(emptyMetadataInfo));
        }

    }
}
