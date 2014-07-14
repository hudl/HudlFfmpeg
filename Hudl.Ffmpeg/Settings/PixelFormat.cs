using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// sets the output pix format type.
    /// </summary>
    [AppliesToResource(Type = typeof(IVideo))]
    [AppliesToResource(Type = typeof(IImage))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class PixelFormat : BaseSetting
    {
        private const string SettingType = "-pix_fmt";

        public PixelFormat(string library)
            : base(SettingType)
        {
            Library = library;
        }
        public PixelFormat(PixelFormatType library)
            : this(Formats.Library(library))
        {
        }

        public string Library { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Library))
            {
                throw new InvalidOperationException("Library cannot be empty for this setting.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Library);
        }
    }
}
