using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters.Templates.Filterchain
{
    public class Resolution720P<TResource> : Filterchain<TResource>
        where TResource : IVideo, new()
    {
        public Resolution720P()
            : base(new TResource(), 
                   new Scale(ScalePresetTypes.Hd720), 
                   new SetDar(new FfmpegRatio(16, 9)), 
                   new SetSar(new FfmpegRatio(1, 1)))
        {
        }
    }
}
