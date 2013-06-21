using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Concatentates multiple resource streams into a collection of output streams
    /// </summary>
    [AppliesToResource(Type=typeof(IAudio))]
    [AppliesToResource(Type=typeof(IVideo))]
    public class Concat : IFilter
    {
        private const int DefaultVideoOut = 1;
        private const int DefaultAudioOut = 0;
        private const int MinimumResourceCount = 2; 
        private const int MaximumResourceCount = 4; 

        public Concat() 
        {
            NumberOfVideoOut = _numberOfVideoOut;
            NumberOfAudioOut = _numberOfAudioOut;
                
        }
        public Concat(int numberOfResources, int numberOfAudioOut, int numberOfVideoOut) 
        {
            NumberOfVideoOut = numberOfAudioOut;
            NumberOfAudioOut = numberOfVideoOut;
            NumberOfResources = numberOfResources;
        }

        public int NumberOfResources 
        {   
            get 
            {
                return _numberOfResources;
            }
            set 
            { 
                if (value < MinimumResourceCount)
                    throw new ArgumentException(string.Format("Number of resources cannot be less than {0}.", MinimumResourceCount));
                if (value > MaximumResourceCount)
                    throw new ArgumentException(string.Format("Number of resources cannot be greater than {0}.", MaximumResourceCount)); 
                _numberOfResources = value;
            } 
        }
        private int _numberOfResources = MinimumResourceCount;

        public int NumberOfVideoOut { get; set; }
        private int _numberOfVideoOut = DefaultVideoOut;
        
        public int NumberOfAudioOut { get; set; }
        private int _numberOfAudioOut = DefaultAudioOut;

        public string Type { get { return "concat"; } }

        public int MaxInputs { get { return MaximumResourceCount; } }

        public override string ToString()
        {
            if (NumberOfVideoOut > NumberOfResources) 
                throw new ArgumentException("Number of Videos out cannot be greater than Resources in.", "NumberOfVideoOut");
            if (NumberOfAudioOut > NumberOfResources) 
                throw new ArgumentException("Number of Audios out cannot be greater than Resources in.", "NumberOfAudioOut");

            StringBuilder filter = new StringBuilder(100);
            if (NumberOfResources > MinimumResourceCount) 
                filter.AppendFormat("{1}n={0}", 
                    NumberOfResources, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            if (NumberOfVideoOut > DefaultVideoOut) 
                filter.AppendFormat("{1}v={0}", 
                    NumberOfVideoOut, 
                    (filter.Length > 0) ?  ":" : string.Empty);
            if (NumberOfAudioOut > DefaultAudioOut) 
                filter.AppendFormat("{1}a={0}", 
                    NumberOfAudioOut, 
                    (filter.Length > 0) ?  ":" : string.Empty);

            return string.Concat(Type, filter.ToString());
        }
    }
}
