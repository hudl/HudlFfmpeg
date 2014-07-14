using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Metadata;
using Hudl.Ffmpeg.Metadata.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Concat Filter concatenates multiple resource streams into a collection of output streams
    /// </summary>
    [AppliesToResource(Type=typeof(IAudio))]
    [AppliesToResource(Type=typeof(IVideo))]
    public class Concat : BaseFilter, IFilterValidator, IMetadataManipulation
    {
        private const int FilterMaxInputs = 4;
        private const string FilterType = "concat";
        private const int DefaultVideoOut = 1;
        private const int DefaultAudioOut = 0;

        public Concat() 
            : base(FilterType, FilterMaxInputs)
        {
            NumberOfVideoOut = DefaultVideoOut;
            NumberOfAudioOut = DefaultAudioOut;
        }
        public Concat(int? numberOfAudioOut, int? numberOfVideoOut)
            : base(FilterType, FilterMaxInputs)
        {
            NumberOfAudioOut = numberOfAudioOut;
            NumberOfVideoOut = numberOfVideoOut;
        }

        public int? NumberOfVideoOut { get; set; }
        
        public int? NumberOfAudioOut { get; set; }

        public bool UnsafeMode { get; set; }

        public override void Validate()
        {
            var numberOfResources = InputCount;
            if (NumberOfVideoOut.HasValue && NumberOfVideoOut > numberOfResources)
            {
                throw new InvalidOperationException("Number of Videos out cannot be greater than Resources in.");
            }
            if (NumberOfAudioOut.HasValue && NumberOfAudioOut > numberOfResources)
            {
                throw new InvalidOperationException("Number of Audios out cannot be greater than Resources in.");
            }
        }

        public override string ToString()
        {
            var numberOfResources = InputCount;

            var filterParameters = new StringBuilder(100);

            if (numberOfResources > 0)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "n", numberOfResources);
            }
            if (NumberOfVideoOut.HasValue) 
            {
                FilterUtility.ConcatenateParameter(filterParameters, "v", NumberOfVideoOut.GetValueOrDefault());
            }
            if (NumberOfAudioOut != DefaultAudioOut)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "a", NumberOfAudioOut.GetValueOrDefault());
            }
            if (UnsafeMode)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "unsafe");
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }

        #region IFilterValidator
        public bool Validate(FfmpegCommand command, Filterchain filterchain, List<CommandReceipt> receipts)
        {
            //concat filters should be used independently of other filters
            return filterchain.Filters.Count == 1;
        }
        #endregion

        public MetadataInfo EditInfo(MetadataInfo infoToUpdate, List<MetadataInfo> suppliedInfo)
        {
            infoToUpdate.Duration = TimeSpan.FromSeconds(suppliedInfo.Sum(i => i.Duration.TotalSeconds));

            return infoToUpdate; 
        }
    }
}
