using System.Collections.Generic;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// sets the video bitrate for the output stream
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "b:v")]
    public class BitRateVideo : BaseBitRate, IMetadataManipulation
    {
        public BitRateVideo(int rate)
            : base(rate)
        {
        }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            infoToUpdate.VideoStream.VideoMetadata.BitRate = Rate * 1000;

            return infoToUpdate;
        }
    }
}
