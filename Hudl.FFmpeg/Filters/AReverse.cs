using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    [ForStream(Type = typeof(AudioStream))]
    [Filter(Name ="areverse", MinInputs = 1, MaxInputs = 1)]
    public class AReverse : IFilter
    {
        //This filter accepts no parameters
    }
}
