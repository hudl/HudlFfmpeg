using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Models;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Resources.BaseTypes
{
    public class DataStream : IStream
    {
        public DataStream()
        {
            Map = Helpers.NewMap();
            Info = MetadataInfo.Create();
        }

        public DataStream(MetadataInfo info)
            : this()
        {
            Info = info;
        }

        public string ResourceIndicator { get { return "d"; } }

        public string Map { get; set; }

        public MetadataInfo Info { get; set; }

        public IStream Copy()
        {
            return new DataStream
            {
                Info = Info
            };
        }

        public static DataStream Create(MetadataInfo info)
        {
            return new DataStream(info);
        }
    }
}
