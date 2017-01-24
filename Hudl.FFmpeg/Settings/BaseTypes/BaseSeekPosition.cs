using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;
using Hudl.FFmpeg.Validators;
using System;
using System.Collections.Generic;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    public abstract class BaseSeekPosition : ISetting, IMetadataManipulation
    {
        protected BaseSeekPosition(TimeSpan length)
        {
            if (length == null)
            {
                throw new ArgumentNullException("length");
            }

            Length = length;
        }

        [SettingParameter(Formatter = typeof(TimeSpanFormatter))]
        [Validate(typeof(TimeSpanGreterThanZeroValidator))]
        public TimeSpan Length { get; set; }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            if (infoToUpdate.HasVideo && infoToUpdate.VideoStream.VideoMetadata.Duration > Length)
            {
                infoToUpdate.VideoStream.VideoMetadata.Duration = infoToUpdate.VideoStream.VideoMetadata.Duration - Length;
            }

            if (infoToUpdate.HasAudio && infoToUpdate.AudioStream.AudioMetadata.Duration > Length)
            {
                infoToUpdate.AudioStream.AudioMetadata.Duration = infoToUpdate.AudioStream.AudioMetadata.Duration - Length;
            }

            return infoToUpdate;
        }
    }

}
