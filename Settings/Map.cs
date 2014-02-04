using System;
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
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class Map : BaseSetting
    {
        private const string SettingType = "-map";

        public Map(CommandReceipt receipt)
            : base(SettingType)
        {
            Stream = receipt.Map;
        }
    
        public string Stream { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Stream))
            {
                throw new InvalidOperationException("Map setting Stream cannot be null.");
            }

            return string.Concat(Type, " ", Formats.Map(Stream));
        }
    }
}
