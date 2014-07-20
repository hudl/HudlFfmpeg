using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Metadata;
using Hudl.Ffmpeg.Metadata.BaseTypes;

namespace Hudl.Ffmpeg.Settings.BaseTypes
{
    public abstract class BaseDuration : BaseSetting, IMetadataManipulation
    {
        private const string SettingType = "-t";

        protected BaseDuration(TimeSpan length)
            : base(SettingType)
        {
            if (length == null)
            {
                throw new ArgumentNullException("length");
            }

            Length = length;
        }
        protected BaseDuration(double seconds)
            : this(TimeSpan.FromSeconds(seconds))
        {
        }

        public TimeSpan Length { get; set; }

        public override void Validate()
        {
            if (Length == null)
            {
                throw new InvalidOperationException("Duration length cannot be null.");
            }
            if (Length.TotalSeconds <= 0)
            {
                throw new InvalidOperationException("Duration length must be greater than zero.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Formats.Duration(Length));
        }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            if (infoToUpdate.VideoStream.Duration > Length)
            {
                infoToUpdate.VideoStream.Duration = Length;
            }

            return infoToUpdate; 
        }
    }
}
