using System.Collections.Generic;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFprobe.Metadata.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// sets the video bitrate for the output stream
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class BitRateVideo : BaseBitRate, IMetadataManipulation
    {
        private const string Suffix = ":v";

        public BitRateVideo(int rate)
            : base(Suffix, rate)
        {
        }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            infoToUpdate.VideoStream.VideoMetadata.BitRate = Rate * 1000;

            return infoToUpdate;
        }
    }
}
