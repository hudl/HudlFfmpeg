using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Sugar
{
    public static class ResourceExtensions
    {
        public static IContainer LoadMetadata(this IContainer resource)
        {
            if (ResourceManagement.CommandConfiguration != null &&
                !string.IsNullOrWhiteSpace(ResourceManagement.CommandConfiguration.FFprobePath))
            {
                return resource.LoadMetadataFromFFprobe();
            }

            return resource; 
        }

        private static IContainer LoadMetadataFromFFprobe(this IContainer resource)
        {
            var mediaLoader = new Metadata.FFprobe.MediaLoader(resource);

            if (mediaLoader.HasAudio)
            {
                var audioStreamMetadata = MetadataInfo.Create(mediaLoader.AudioStream);

                resource.Streams.Add(AudioStream.Create(audioStreamMetadata));
            }

            if (mediaLoader.HasVideo)
            {
                var videoStreamMetadata = MetadataInfo.Create(mediaLoader.VideoStream);

                resource.Streams.Add(VideoStream.Create(videoStreamMetadata));
            }

            return resource;
        }
    }
}
