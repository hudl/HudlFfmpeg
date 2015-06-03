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
    /// Seek to should be used when StartAt cannot be used, FFmpeg will process the video up to the timestamp provided, but discard it. 
    /// </summary>
    [ForStream(Type = typeof(AudioStream))]
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "ss", IsPreDeclaration = false, ResourceType = SettingsCollectionResourceType.Input)]
    public class SeekTo : ISetting, IMetadataManipulation
    {
        public SeekTo(TimeSpan length)
        {
            if (length == null)
            {
                throw new ArgumentNullException("length");
            }

            Length = length;
        }
        public SeekTo(double seconds)
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
