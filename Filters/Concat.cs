using System;
using System.Collections.Generic;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Concat Filter concatenates multiple resource streams into a collection of output streams
    /// </summary>
    [AppliesToResource(Type=typeof(IAudio))]
    [AppliesToResource(Type=typeof(IVideo))]
    public class Concat : BaseFilter, IFilterValidator
    {
        private const int FilterMinInputs = 2;
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
        public Concat(int numberOfAudioOut, int numberOfVideoOut)
            : base(FilterType, FilterMaxInputs)
        {
            NumberOfAudioOut = numberOfAudioOut;
            NumberOfVideoOut = numberOfVideoOut;
        }

        public int NumberOfVideoOut { get; set; }
        
        public int NumberOfAudioOut { get; set; }

        public override string ToString()
        {
            var numberOfResources = CommandResources.Count; 
            if (NumberOfVideoOut > numberOfResources)
            {
                throw new InvalidOperationException("Number of Videos out cannot be greater than Resources in.");
            }
            if (NumberOfAudioOut > numberOfResources) 
            {
                throw new InvalidOperationException("Number of Audios out cannot be greater than Resources in.");
            }
            
            var filter = new StringBuilder(100);
            if (numberOfResources > FilterMinInputs)
            {
                filter.AppendFormat("{1}n={0}", 
                    numberOfResources, 
                    (filter.Length > 0) ?  ":" : "=");
            }
            if (NumberOfVideoOut != DefaultVideoOut) 
            {
                filter.AppendFormat("{1}v={0}", 
                    NumberOfVideoOut,
                    (filter.Length > 0) ? ":" : "=");
            }
            if (NumberOfAudioOut != DefaultAudioOut)
            {
                filter.AppendFormat("{1}a={0}", 
                    NumberOfAudioOut,
                    (filter.Length > 0) ? ":" : "=");
            }

            return string.Concat(Type, filter.ToString());
        }

        #region IFilterValidator
        public bool Validate(FfmpegCommand command, Filterchain filterchain, List<CommandReceipt> receipts)
        {
            //concat filters should be used independently of other filters
            return filterchain.Filters.Count == 1;
        }
        #endregion
    }
}
