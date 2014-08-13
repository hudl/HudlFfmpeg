using System;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// sets the output pix format type.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
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
