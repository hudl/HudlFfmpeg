using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsApplicationAttribute.SettingsResourceType.Output)]
    public class PixelFormat : ISetting
    {
        public PixelFormat(string library)
        {
            if (string.IsNullOrWhiteSpace(library))
            {
                throw new ArgumentNullException("library");
            }

            Library = library;
        }
        public PixelFormat(PixelFormatTypes library)
            : this(Formats.Library(library))
        {
        }

        public string Library { get; set; }

        public string Type { get { return "-pix_fmt"; } }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Library))
            {
                throw new ArgumentException("Library cannot be empty for this setting.");
            }

            return string.Concat(Type, " ", Library);
        }
    }
}
