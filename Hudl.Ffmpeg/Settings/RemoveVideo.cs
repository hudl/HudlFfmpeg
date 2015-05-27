using System.Collections.Generic;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// removes the video stream from the output file
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "vn", IsParameterless = true)]
    public class RemoveVideo : ISetting, IMetadataManipulation
    {
        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            infoToUpdate.VideoStream = null;

            return infoToUpdate;
        }
    }
}
