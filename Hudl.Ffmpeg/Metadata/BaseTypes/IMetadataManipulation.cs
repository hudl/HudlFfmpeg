using System.Collections.Generic;

namespace Hudl.Ffmpeg.Metadata.BaseTypes
{
    public interface IMetadataManipulation
    {
        /// <summary>
        /// an interface that can be attached to settings and to filters to modify and edit the media info of any resource
        /// </summary>
        MetadataInfo EditInfo(MetadataInfo infoToUpdate, List<MetadataInfo> suppliedInfo);
    }
}
