using Hudl.Ffmpeg.Metadata;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Sugar
{
    public static class ResourceExtensions
    {
        public static IContainer LoadMetadata(this IContainer resource)
        {
            if (ResourceManagement.CommandConfiguration != null &&
                !string.IsNullOrWhiteSpace(ResourceManagement.CommandConfiguration.FfprobePath))
            {
                return resource.LoadMetadataFromFfprobe();
            }

            return resource; 
        }

        private static IContainer LoadMetadataFromFfprobe(this IContainer resource)
        {
            var mediaLoader = new Metadata.Ffprobe.MediaLoader(resource);

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
