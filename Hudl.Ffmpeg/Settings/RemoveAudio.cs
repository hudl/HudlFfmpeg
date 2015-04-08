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
    /// removes the audio stream from the output file
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class RemoveAudio : BaseSetting, IMetadataManipulation
    {
        private const string SettingType = "-an";

        public RemoveAudio()
            : base(SettingType)
        {
        }
        
        public override string ToString()
        {
            return Type;
        }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            infoToUpdate.AudioStream = null;

            return infoToUpdate;
        }
    }
}
