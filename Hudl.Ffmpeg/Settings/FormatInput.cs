using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// Force input or output file format. The format is normally auto detected for input files and guessed from the file extension for output files, so this option is not needed in most cases.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [ForStream(Type = typeof(AudioStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Input)]
    public class FormatInput : BaseFormat
    {
        public FormatInput(string format)
            : base(format)
        {
        }

        public FormatInput(FormatType format)
            : base(format)
        {
        }
    }
}