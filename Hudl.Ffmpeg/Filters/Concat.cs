using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Filters.Bindings;
using Hudl.FFmpeg.Filters.Contexts;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Concat Filter concatenates multiple resource streams into a collection of output streams
    /// </summary>
    [ForStream(Type=typeof(AudioStream))]
    [ForStream(Type=typeof(VideoStream))]
    [Filter(Name = "concat", MinInputs = 2, MaxInputs = 4)]
    public class Concat : 
        IFilter, 
        IFilterValidator, 
        IMetadataManipulation
    {
        private const int DefaultVideoOut = 1;
        private const int DefaultAudioOut = 0;

        public Concat() 
        {
            NumberOfVideoOut = DefaultVideoOut;
            NumberOfAudioOut = DefaultAudioOut;
        }
        public Concat(int? numberOfAudioOut, int? numberOfVideoOut)
        {
            NumberOfAudioOut = numberOfAudioOut;
            NumberOfVideoOut = numberOfVideoOut;
        }

        [FilterParameter(Name="n", Binding = typeof(NumberOfStreamsInBinding))]
        public int? NumberOfResources { get { return null; } }

        [FilterParameter(Name ="v", Default = DefaultVideoOut)]
        public int? NumberOfVideoOut { get; set; }

        [FilterParameter(Name="a", Default = DefaultAudioOut)]
        public int? NumberOfAudioOut { get; set; }

        [FilterParameter(Name = "unsafe", Default = false, ShouldHideValue = true)] 
        public bool UnsafeMode { get; set; }

        #region IFilterValidator
        public bool Validate(FilterValidatorContext context)
        {
            return context.NumberOfFiltersInFilterchain == 1;
        }
        #endregion

        #region IMetadataManipulation
        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            if (NumberOfAudioOut > 0)
            {
                infoToUpdate.AudioStream.AudioMetadata.Duration = TimeSpan.FromSeconds(suppliedInfo.Sum(i => i.AudioStream.AudioMetadata.Duration.TotalSeconds));
            }

            if (NumberOfVideoOut > 0)
            {
                infoToUpdate.VideoStream.VideoMetadata.Duration = TimeSpan.FromSeconds(suppliedInfo.Sum(i => i.VideoStream.VideoMetadata.Duration.TotalSeconds));
            }

            return infoToUpdate;
        }
        #endregion
    }
}
