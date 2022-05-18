using System;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;
using Hudl.FFmpeg.Validators;

namespace Hudl.FFmpeg.Settings{

        /// <summary>
        /// Set Encoding Preset
        /// </summary>
        [ForStream(Type = typeof(VideoStream))]
        [Setting(Name = "preset")]
        public class Preset : ISetting
        {

            public Preset(PresetType preset)
            {
                PresetType = preset;
            }

            [SettingParameter(Formatter = typeof(EnumParameterFormatter))]
            [Validate(typeof(NullOrWhitespaceValidator))]
            public PresetType PresetType { get; set; }
        } 
    }