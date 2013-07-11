using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// Seek to should be used when StartAt cannot be used, Ffmpeg will process the video up to the timestamp provided, but discard it. 
    /// </summary>
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = false, ResourceType = SettingsCollectionResourceType.Input)]
    public class SeekTo : BaseSetting
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

        public override TimeSpan? LengthFromInputs(System.Collections.Generic.List<Command.CommandResource<IResource>> resources)
        {
            var overallLength = TimeSpan.FromSeconds(0);
            var baseCalculatedLength = base.LengthFromInputs(resources);
            if (baseCalculatedLength == null)
            {
                return overallLength;
            }
            return baseCalculatedLength - Length;
        }

        public override string ToString()
        {
            if (Length == null)
            {
                throw new InvalidOperationException("SeekTo length cannot be null.");
            }
            if (Length.TotalSeconds <= 0)
            {
                throw new InvalidOperationException("SeekTo length must be greater than zero.");
            }

            return string.Concat(Type, " ", Formats.Duration(Length));
        }
    }
}
