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
    /// removes the video stream from the output file
    /// </summary>
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class RemoveVideo : BaseSetting, IMetadataManipulation
    {
        private const string SettingType = "-vn";

        public RemoveVideo()
            : base(SettingType)
        {
        }
        
        public override string ToString()
        {
            return Type;
        }
       
        public MetadataInfo EditInfo(MetadataInfo infoToUpdate, List<MetadataInfo> suppliedInfo)
        {
            infoToUpdate.HasVideo = false;

            return infoToUpdate;
        }
    }
}
