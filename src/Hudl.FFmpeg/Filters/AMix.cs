using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Filter that mixes multiple audio signals into a single audio source 
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [Filter(Name = "amix", MinInputs = 2, MaxInputs = 4)]
    public class AMix : IFilter, IMetadataManipulation
    {
        public AMix() 
        {
        }
        public AMix(int? inputs, double? dropoutTransition, DurationType duration)
            : this()
        {
            Inputs = inputs;
            Duration = duration; 
            DropoutTransition = dropoutTransition;
        }

        [FilterParameter(Name = "inputs")]
        public int? Inputs { get; set; }

        [FilterParameter(Name = "dropout_transition")]
        public double? DropoutTransition { get; set; }

        [FilterParameter(Name = "duration", Default = DurationType.Longest, Formatter = typeof(EnumParameterFormatter))]
        public DurationType? Duration { get; set; }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            switch (Duration)
            {
                case DurationType.First:
                    return suppliedInfo.FirstOrDefault();
                case DurationType.Shortest:
                    return suppliedInfo.OrderBy(r => r.AudioStream.AudioMetadata.Duration).FirstOrDefault();
                default:
                    return suppliedInfo.OrderByDescending(r => r.AudioStream.AudioMetadata.Duration).FirstOrDefault();
            }
        }
    }
}
