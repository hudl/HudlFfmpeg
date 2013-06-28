using System;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// CConcat Filter concatentates multiple resource streams into a collection of output streams
    /// </summary>
    [AppliesToResource(Type=typeof(IAudio))]
    [AppliesToResource(Type=typeof(IVideo))]
    public class Concat : BaseFilter, IFilterPreProcessor
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
            NumberOfVideoOut = numberOfAudioOut;
            NumberOfAudioOut = numberOfVideoOut;
        }

        public int NumberOfVideoOut { get; set; }
        
        public int NumberOfAudioOut { get; set; }

        public override string ToString()
        {
            var numberOfResources = Resources.Count; 
            if (NumberOfVideoOut > numberOfResources)
            {
                throw new ArgumentException("Number of Videos out cannot be greater than Resources in.", "NumberOfVideoOut");
            }
            if (NumberOfAudioOut > numberOfResources) 
            {
                throw new ArgumentException("Number of Audios out cannot be greater than Resources in.", "NumberOfAudioOut");
            }

            var filter = new StringBuilder(100);
            if (numberOfResources > FilterMinInputs)
            {
                filter.AppendFormat("{1}n={0}", 
                    numberOfResources, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            }
            if (NumberOfVideoOut > DefaultVideoOut) 
            {
                filter.AppendFormat("{1}v={0}", 
                    NumberOfVideoOut, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            }
            if (NumberOfAudioOut > DefaultAudioOut)
            {
                filter.AppendFormat("{1}a={0}", 
                    NumberOfAudioOut, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            }

            return string.Concat(Type, filter.ToString());
        }
    }
}
