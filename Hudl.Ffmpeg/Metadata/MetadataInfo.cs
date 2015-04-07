using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes.Models;

namespace Hudl.FFmpeg.Metadata
{
    public class MetadataInfo
    {
        private MetadataInfo()
        {
        }

        public bool HasAudio { get { return AudioMetadata != null; } }

        public bool HasVideo { get { return VideoMetadata != null; } }

        public AudioStreamMetadata AudioMetadata { get; internal set; }

        public VideoStreamMetadata VideoMetadata { get; internal set; }

        public MetadataInfo Copy()
        {
            return new MetadataInfo
                {
                    AudioMetadata = HasAudio ? AudioMetadata.Copy() : null,
                    VideoMetadata = HasVideo ? VideoMetadata.Copy() : null,
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
    }
}
