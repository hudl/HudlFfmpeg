using System;
using System.Collections.Generic;
using System.Drawing;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class Gop : BaseSetting
    {
        private const string SettingType = "-g";

        public Gop(int gopSize)
            : base(SettingType)
        {
            Size = gopSize;
        }

        public int Size { get; set; }

        public override string ToString()
        {
            if (Size <= 0)
            {
                throw new InvalidOperationException("Gop size must be greater than zero.");
            }

            return string.Concat(Type, " ", Size);
        }
    }
}
