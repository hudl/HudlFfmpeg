using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// Set frame rate (Hz value, fraction or abbreviation).
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "loop", IsPreDeclaration = true, ResourceType = SettingsCollectionResourceType.Input)]
    public class Loop : ISetting
    {
        public Loop(int value)
        {
            Value = value;
        }

        [SettingParameter]
        public int Value { get; set; }
    }
}
