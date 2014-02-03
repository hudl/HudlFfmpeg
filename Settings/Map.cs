using System;
using System.Collections.Generic;
using System.Drawing;
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
            Receipt = receipt;
        }
    
        public CommandReceipt Receipt { get; set; }

        public override string ToString()
        {
            if (Receipt == null)
            {
                throw new InvalidOperationException("Map setting receipt cannot be null.");
            }

            return string.Concat(Type, " ", Formats.Map(Receipt.Map));
        }
    }
}
