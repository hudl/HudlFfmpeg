using System;
using System.Collections.Generic;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Start At can only be used on the first input resource stream. FFmpeg will not process the video until the starting point provided.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Input)]
    public class StartAt : BaseSetting, IMetadataManipulation
    {
        private const string SettingType = "-ss";
        
        public StartAt(TimeSpan length)
            : base(SettingType)
        {
            if (length == null)
            {
                throw new ArgumentNullException("length");
            }

            Length = length;
        }
        public StartAt(double seconds)
            : this(TimeSpan.FromSeconds(seconds))
        {
        }

        public TimeSpan Length { get; set; }

        public override void Validate()
        {
            if (Length == null)
            {
                throw new InvalidOperationException("StartAt length cannot be null.");
            }
            if (Length.TotalSeconds <= 0)
            {
                throw new InvalidOperationException("StartAt length must be greater than zero.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Formats.Duration(Length));
        }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            infoToUpdate.VideoStream.VideoMetadata.Duration = infoToUpdate.VideoStream.VideoMetadata.Duration - Length;

            return infoToUpdate;
        }
    }
}
