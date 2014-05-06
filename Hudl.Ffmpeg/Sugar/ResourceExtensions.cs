using Hudl.Ffmpeg.MediaInfo;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Sugar
{
    public static class ResourceExtensions
    {
        public static IResource LoadMetadata(this IResource resource)
        {
            var mediaLoader = new MediaLoader(resource.FullName);

            resource.Info = MetadataInfo.Create(mediaLoader);

            return resource;
        }
    }
}
