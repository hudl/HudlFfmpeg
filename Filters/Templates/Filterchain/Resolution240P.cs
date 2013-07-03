using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.Templates.Filterchain
{
    public class Resolution240P<TResource> : Filterchain<TResource>
        where TResource : IVideo, new()
    {
        public Resolution240P()
            : base(new TResource(), 
                   new Scale(ScalePresetTypes.Sd240), 
                   new SetDar(new FfmpegRatio(16, 9)), 
                   new SetSar(new FfmpegRatio(1, 1)))
        {
        }
    }
}
