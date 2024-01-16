using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Metadata.Models;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources.BaseTypes
{
    public class AudioStream : IStream
    {
        public AudioStream()
        {
            Map = Helpers.NewMap();
            Info = MetadataInfo.Create();
        }

        public AudioStream(MetadataInfo info)
            : this()
        {
            Info = info;
        }

        public string ResourceIndicator { get { return "a"; } }

        public string Map { get; set; }

        public MetadataInfo Info { get; set; }

        public IStream Copy()
        {
            return new AudioStream
            {
                Info = Info
            };
        }

        public static AudioStream Create(MetadataInfo info)
        {
            return new AudioStream(info);
        }
    }
}
