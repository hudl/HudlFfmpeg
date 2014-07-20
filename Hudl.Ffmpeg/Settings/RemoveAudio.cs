using System.Collections.Generic;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Metadata;
using Hudl.Ffmpeg.Metadata.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
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
            infoToUpdate.HasAudio = false;

            infoToUpdate.AudioStream = null;

            return infoToUpdate;
        }
    }
}
