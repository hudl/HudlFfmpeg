using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Metadata;

namespace Hudl.Ffmpeg.Resources.BaseTypes
{
    public class VideoStream : IStream
    {
        public VideoStream()
        {
            Map = Helpers.NewMap();
            Info = MetadataInfo.Create();
        }

        private VideoStream(MetadataInfo info)
            : this()
        {
            Info = info;
        }

        public string ResourceIndicator { get { return "v"; } }

        public string Map { get; set; }

        public MetadataInfo Info { get; set; }

        public IStream Copy()
        {
            return new VideoStream
                {
                    Info = Info
                };
        }

        public static VideoStream Create(MetadataInfo info)
        {
            return new VideoStream(info);
        }
    }
}
