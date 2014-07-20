using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// input file name
    /// </summary>
    [ForStream(Type = typeof(IContainer))]
    internal class Input : BaseSetting
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
