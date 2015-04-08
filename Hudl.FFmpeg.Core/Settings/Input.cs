using System;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Models;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// input file name
    /// </summary>
    public class Input : BaseSetting
    {
        private const string SettingType = "-i";

        public Input(IContainer resource)
            : base(SettingType)
        {
            Resource = resource; 
        }

        public IContainer Resource { get; protected set; }

        public override void Validate()
        {
            if (Resource == null)
            {
                throw new InvalidOperationException("Resource cannot be empty.");
            }
        }

        public override string ToString()
        {
            var escapedPath = Resource.FullName.Replace('\\', '/');
            return string.Concat(Type, " \"", escapedPath, "\"");
        }
    }
}
