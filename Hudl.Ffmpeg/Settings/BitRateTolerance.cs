using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// sets the bitrate tolerance for the output stream 
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class BitRateTolerance : BaseBitRate
    {
        private const string Suffix = "t";

        public BitRateTolerance(int rate)
            : base(Suffix, rate)
        {
        }
    }
}
