using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IAudio))]
    [AppliesToResource(Type = typeof(IVideo))]
    [AppliesToResource(Type = typeof(IImage))]
    [SettingsApplication(PreDeclaration = true, MultipleAllowed = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class Map : BaseSetting
    {
        private const string SettingType = "-map";

        public Map(CommandReceipt receipt)
            : base(SettingType)
        {
            Stream = receipt.Map;
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
