using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IResource))]
    internal class Input : BaseSetting
    {
        private const string SettingType = "-i";

        public Input(IResource resource)
            : base(SettingType)
        {
            Resource = resource; 
        }

        public IResource Resource { get; protected set; }
        
        public override string ToString()
        {
            if (Resource == null)
            {
                throw new InvalidOperationException("Resource cannot be empty.");
            }

            var escapedPath = Resource.FullName.Replace('\\', '/');
            return string.Concat(Type, " \"", escapedPath, "\"");
        }
    }
}
