using System.Collections.Generic;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Filters.Contexts
{
    public class FilterProcessorContext
    {
        public List<IStream> Streams { get; set; }

        public IFilterchain Filterchain { get; set; }
    }
}
