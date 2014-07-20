using System;
using System.Drawing;
using System.Linq;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Metadata.Ffprobe;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Metadata
{
    public class MetadataInfoTreeContainer
    {
        private MetadataInfoTreeContainer()
        {
        }

        public bool HasAudio { get; internal set; }

        public bool HasVideo { get; internal set; }

        public MetadataInfo AudioStream { get; internal set; }

        public MetadataInfo VideoStream { get; internal set; }

        public MetadataInfoTreeContainer Copy()
        {
            return new MetadataInfoTreeContainer
                {
                    HasAudio = HasAudio,
                    HasVideo = HasVideo,
                    AudioStream = HasAudio ? AudioStream.Copy() : null,
                    VideoStream = HasVideo ? VideoStream.Copy() : null
                };
        }

        internal static MetadataInfoTreeContainer Create(IContainer container)
        {
            var instanceOfTreeContainer = new MetadataInfoTreeContainer();

            if (container.Streams.OfType<VideoStream>().Any())
            {
                instanceOfTreeContainer.HasVideo = true;

                instanceOfTreeContainer.VideoStream = container.Streams.OfType<VideoStream>().First().Info.Copy();
            }

            if (container.Streams.OfType<AudioStream>().Any())
            {
                instanceOfTreeContainer.HasAudio = true;

                instanceOfTreeContainer.AudioStream = container.Streams.OfType<AudioStream>().First().Info.Copy();
            }

            return instanceOfTreeContainer;
        }

        internal static MetadataInfoTreeContainer Create(VideoStream stream)
        {
            return new MetadataInfoTreeContainer
                {
                    HasVideo = true, 
                    VideoStream = stream.Info.Copy()
                };
        }

        internal static MetadataInfoTreeContainer Create(AudioStream stream)
        {
            return new MetadataInfoTreeContainer
                {
                    HasAudio = true,
                    AudioStream = stream.Info.Copy()
                };
        }
    }
}
