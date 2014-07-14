using Hudl.Ffmpeg.Metadata;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Sugar
{
    public static class ResourceExtensions
    {
        public static IResource LoadMetadata(this IResource resource)
        {
            if (ResourceManagement.CommandConfiguration != null &&
                !string.IsNullOrWhiteSpace(ResourceManagement.CommandConfiguration.FfprobePath))
            {
                return resource.LoadMetadataFromFfprobe();
            }

            return resource.LoadMetadataFromMediaInfo();
        }

        private static IResource LoadMetadataFromMediaInfo(this IResource resource)
        {
            var mediaLoader = new Metadata.MediaInfo.MediaLoader(resource.FullName);

            resource.Info = MetadataInfo.Create(mediaLoader);

            return resource;
        }

        private static IResource LoadMetadataFromFfprobe(this IResource resource)
        {
            var mediaLoader = new Metadata.Ffprobe.MediaLoader(resource);

            resource.Info = MetadataInfo.Create(mediaLoader);

            return resource;
        }
    }
}
