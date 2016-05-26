using Hudl.FFprobe.Metadata.Models;

namespace Hudl.FFmpeg.Metadata.Models
{
    public class MetadataInfo
    {
        private MetadataInfo()
        {
        }

        public bool HasAudio { get { return AudioMetadata != null; } }

        public bool HasVideo { get { return VideoMetadata != null; } }

        public bool HasData { get { return DataMetadata != null; } }

        public AudioStreamMetadata AudioMetadata { get; internal set; }

        public VideoStreamMetadata VideoMetadata { get; internal set; }

        public DataStreamMetadata DataMetadata { get; internal set; }

        public MetadataInfo Copy()
        {
            return new MetadataInfo
            {
                AudioMetadata = HasAudio ? AudioMetadata.Copy() : null,
                VideoMetadata = HasVideo ? VideoMetadata.Copy() : null,
                DataMetadata = HasData ? DataMetadata.Copy() : null,
            };
        }

        internal static MetadataInfo Create()
        {
            return new MetadataInfo();
        }

        internal static MetadataInfo Create(AudioStreamMetadata loader)
        {
            return new MetadataInfo
            {
                AudioMetadata = loader,
            };
        }

        internal static MetadataInfo Create(VideoStreamMetadata loader)
        {
            return new MetadataInfo
            {
                VideoMetadata = loader,
            };
        }

        internal static MetadataInfo Create(DataStreamMetadata loader)
        {
            return new MetadataInfo
            {
                DataMetadata = loader,
            };
        }
    }
}
