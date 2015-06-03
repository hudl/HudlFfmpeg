using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;
using Hudl.FFmpeg.Validators;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Start At can only be used on the first input resource stream. FFmpeg will not process the video until the starting point provided.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "ss", ResourceType = SettingsCollectionResourceType.Input)]
    public class StartAt : ISetting, IMetadataManipulation
    {
        public StartAt(TimeSpan length)
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

        [SettingParameter(Formatter = typeof(TimeSpanFormatter))]
        [Validate(typeof(TimeSpanGreterThanZeroValidator))]
        public TimeSpan Length { get; set; }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            infoToUpdate.VideoStream.VideoMetadata.Duration = infoToUpdate.VideoStream.VideoMetadata.Duration - Length;

            return infoToUpdate;
        }
    }
}
