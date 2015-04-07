using System.Linq;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.FFprobe.BaseTypes.Models;
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
                resource.Streams.AddRange(mediaLoader.BaseData.Streams
                    .OfType<AudioStreamMetadata>()
                    .Select(audioMetadata => AudioStream.Create(MetadataInfo.Create(audioMetadata))));
            }

            if (mediaLoader.HasVideo)
            {
                resource.Streams.AddRange(mediaLoader.BaseData.Streams
                    .OfType<VideoStreamMetadata>()
                    .Select(videoMetadata => VideoStream.Create(MetadataInfo.Create(videoMetadata))));
            }

            return resource;
        }
    }
}
