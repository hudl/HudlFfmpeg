using System.Linq;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Metadata
{
    public class MetadataInfoTreeContainer
    {
        private MetadataInfoTreeContainer()
        {
        }

        public bool HasAudio { get { return AudioStream != null; } }

        public bool HasVideo { get { return VideoStream != null; } }

        public MetadataInfo AudioStream { get; internal set; }

        public MetadataInfo VideoStream { get; internal set; }

        public MetadataInfoTreeContainer Copy()
        {
            return new MetadataInfoTreeContainer
                {
                    AudioStream = HasAudio ? AudioStream.Copy() : null,
                    VideoStream = HasVideo ? VideoStream.Copy() : null
                };
        }

        internal static MetadataInfoTreeContainer Create(IContainer container)
        {
            var instanceOfTreeContainer = new MetadataInfoTreeContainer();

            if (container.Streams.OfType<VideoStream>().Any())
            {
                instanceOfTreeContainer.VideoStream = container.Streams.OfType<VideoStream>().First().Info.Copy();
            }

            if (container.Streams.OfType<AudioStream>().Any())
            {
                instanceOfTreeContainer.AudioStream = container.Streams.OfType<AudioStream>().First().Info.Copy();
            }

            return instanceOfTreeContainer;
        }

        internal static MetadataInfoTreeContainer Create(VideoStream stream)
        {
            return new MetadataInfoTreeContainer
                {
                    VideoStream = stream.Info.Copy()
                };
        }

        internal static MetadataInfoTreeContainer Create(AudioStream stream)
        {
            return new MetadataInfoTreeContainer
                {
                    AudioStream = stream.Info.Copy()
                };
        }
    }
}
