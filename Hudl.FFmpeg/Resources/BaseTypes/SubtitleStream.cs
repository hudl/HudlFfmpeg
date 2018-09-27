using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Metadata.Models;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources.BaseTypes
{
    public class SubtitleStream : IStream
    {
        public SubtitleStream()
        {
            Map = Helpers.NewMap();
            Info = MetadataInfo.Create();
        }

        private SubtitleStream(MetadataInfo info)
            : this()
        {
            Info = info;
        }

        public string ResourceIndicator { get { return "s"; } }

        public string Map { get; set; }

        public MetadataInfo Info { get; set; }

        public IStream Copy()
        {
            return new SubtitleStream
            {
                    Info = Info
                };
        }

        public static SubtitleStream Create(MetadataInfo info)
        {
            return new SubtitleStream(info);
        }
    }
}
