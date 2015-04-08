using System.Collections.Generic;
using Hudl.FFmpeg.Metadata;

namespace Hudl.FFprobe.Metadata.BaseTypes
{
    public interface IMetadataManipulation
    {
        /// <summary>
        /// an interface that can be attached to settings and to filters to modify and edit the media info of any resource
        /// </summary>
        MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo);
    }
}
