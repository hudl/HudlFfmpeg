using System;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// input file name
    /// </summary>
    [Setting(Name = "i", IsMultipleAllowed = true)]
    public class Input : ISetting
    {
        public Input(IContainer resource)
        {
            Resource = resource; 
        }

        public IContainer Resource { get; protected set; }

        public override string ToString()
        {
            var escapedPath = Resource.FullName.Replace('\\', '/');
            return string.Concat(Type, " \"", escapedPath, "\"");
        }
    }
}
