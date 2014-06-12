using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// Start At can only be used on the first input resource stream. Ffmpeg will not process the video until the starting point provided.
    /// </summary>
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Input)]
    public class StartAt : BaseSetting
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

        public override TimeSpan? LengthFromInputs(System.Collections.Generic.List<CommandResource> resources)
        {
            var overallLength = TimeSpan.FromSeconds(0);
            var baseCalculatedLength = base.LengthFromInputs(resources);
            if (baseCalculatedLength == null)
            {
                return overallLength;
            }
            return baseCalculatedLength - Length;
        }

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
    }
}
