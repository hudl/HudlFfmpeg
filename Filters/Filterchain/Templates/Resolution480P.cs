using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.Filterchain.Templates
{
    public class Resolution480P<TResource> : Filterchain<TResource>
        where TResource : IVideo, new()
    {
        public Resolution480P()
            : base(new TResource(), 
                   new Scale(ScalePresetTypes.Hd480), 
                   new SetDar(new FfmpegRatio(16, 9)), 
                   new SetSar(new FfmpegRatio(1, 1)))
        {
        }
    }
}
