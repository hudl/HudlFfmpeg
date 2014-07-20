using System;
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
    /// Seek to should be used when StartAt cannot be used, Ffmpeg will process the video up to the timestamp provided, but discard it. 
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [SettingsApplication(PreDeclaration = false, ResourceType = SettingsCollectionResourceType.Input)]
    public class SeekTo : BaseSetting, IMetadataManipulation
    {
        private const string SettingType = "-ss";
        
        public SeekTo(TimeSpan length)
            : base(SettingType)
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

        public TimeSpan Length { get; set; }

        public override void Validate()
        {
            if (Length == null)
            {
                throw new InvalidOperationException("SeekTo length cannot be null.");
            }
            if (Length.TotalSeconds <= 0)
            {
                throw new InvalidOperationException("SeekTo length must be greater than zero.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Formats.Duration(Length));
        }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            infoToUpdate.VideoStream.Duration = infoToUpdate.VideoStream.Duration - Length;

            return infoToUpdate; 
        }
    }
}
