using System.Linq;
using Hudl.FFmpeg.Metadata.Models;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFprobe;
using Hudl.FFprobe.Metadata.Models;

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
            var mediaLoader = new MediaLoader(resource);

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

            if (mediaLoader.HasSubtitles)
            {
                resource.Streams.AddRange(mediaLoader.BaseData.Streams
                    .OfType<SubtitleStreamMetadata>()
                    .Select(subtitleMetadata => SubtitleStream.Create(MetadataInfo.Create(subtitleMetadata))));
            }

            if (mediaLoader.HasData)
            {
                resource.Streams.AddRange(mediaLoader.BaseData.Streams
                    .OfType<DataStreamMetadata>()
                    .Select(dataMetadata => DataStream.Create(MetadataInfo.Create(dataMetadata))));
            }

            return resource;
        }
    }
}
