﻿using System;

namespace Hudl.FFmpeg.Filters.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FilterParameterAttribute : Attribute
    {
        public FilterParameterAttribute()
        {
        }
        public FilterParameterAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public object Default { get; set; }

        public bool ShouldHideName { get; set; }

        public bool ShouldHideValue { get; set; }

        public Type Formatter { get; set; }

        public Type Binding { get; set; }

        public int Order { get; set; }
    }
}
