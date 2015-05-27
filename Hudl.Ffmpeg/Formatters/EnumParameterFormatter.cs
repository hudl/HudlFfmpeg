﻿using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Interfaces;

namespace Hudl.FFmpeg.Formatters
{
    public class EnumParameterFormatter : IFormatter
    {
        public string Format(object value)
        {
            return Formats.EnumValue(value);
        }
    }
}