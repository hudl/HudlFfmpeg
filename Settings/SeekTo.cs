using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = false, ResourceType = SettingsApplicationAttribute.SettingsResourceType.Input)]
    public class SeekTo : ISetting
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

        public TimeSpan Length { get; set; }

        public string Type { get { return "-ss"; } }
        
        public override string ToString()
        {
            if (Length == null)
            {
                throw new ArgumentException("SeekTo length cannot be null.");
            }
            if (Length.TotalSeconds <= 0)
            {
                throw new ArgumentException("SeekTo length must be greater than zero.");
            }

            return string.Concat(Type, " ", Formats.Duration(Length));
        }
    }
}
