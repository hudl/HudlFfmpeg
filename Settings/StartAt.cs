using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceTypes.Input)]
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
                throw new InvalidOperationException("StartAt length cannot be null.");
            }
            if (Length.TotalSeconds <= 0)
            {
                throw new InvalidOperationException("StartAt length must be greater than zero.");
            }

            return string.Concat(Type, " ", Formats.Duration(Length));
        }
    }
}
