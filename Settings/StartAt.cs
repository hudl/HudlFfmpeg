using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsApplicationAttribute.SettingsResourceType.Input)]
    public class StartAt : ISetting
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

        public TimeSpan Length { get; set; }

        public string Type { get { return "-ss"; } }
        
        public override string ToString()
        {
            if (Length == null)
            {
                throw new ArgumentException("StartAt length cannot be null.");
            }
            if (Length.TotalSeconds <= 0)
            {
                throw new ArgumentException("StartAt length must be greater than zero.");
            }

            return string.Concat(Type, " ", Formats.Duration(Length));
        }
    }
}
