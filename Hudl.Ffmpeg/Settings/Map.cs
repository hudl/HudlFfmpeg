using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// Designate one or more input streams as a source for the output file.
    /// </summary>
    [ForStream(Type = typeof(IContainer))]
    [SettingsApplication(PreDeclaration = true, MultipleAllowed = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class Map : BaseSetting
    {
        private const string SettingType = "-map";

        public Map(StreamIdentifier streamId)
            : base(SettingType)
        {
            Stream = streamId.Map;
        }

        public Map(string streamId)
            : base(SettingType)
        {
            Stream = streamId;
        }
    
        public string Stream { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Stream))
            {
                throw new InvalidOperationException("Map setting Stream cannot be null.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Formats.Map(Stream, true));
        }
    }
}
