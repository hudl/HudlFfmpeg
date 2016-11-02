using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Filters.Attributes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Pad the end of an audio stream with silence
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [Filter(Name = "apad", MinInputs = 1, MaxInputs = 1)]
    public class APad : IFilter
    {
        public APad() 
        {
        }

        public APad(int? packetSize, int? padLength, int? wholeLength)
            : this()
        {
            PadLength = padLength;
            PacketSize = packetSize;
            WholeLength = wholeLength;
        }

        [FilterParameter(Name = "packet_size")]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public int? PacketSize { get; set; }

        [FilterParameter(Name = "pad_len")]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public int? PadLength { get; set; }

        [FilterParameter(Name = "whole_len")]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public int? WholeLength { get; set; }
    }
}
