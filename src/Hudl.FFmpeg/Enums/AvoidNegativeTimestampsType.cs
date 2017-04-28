using Hudl.FFmpeg.Attributes;

namespace Hudl.FFmpeg.Enums
{
    /// <summary>
    /// When shifting is enabled, all output timestamps are shifted by the same amount. Audio, video, and subtitles desynching and relative timestamp differences are preserved compared to how they would have been without shifting.
    /// </summary>
    public enum AvoidNegativeTimestampsType
    {
        /// <summary>
        /// Shift timestamps to make them non-negative. Also note that this affects only leading negative timestamps, and not non-monotonic negative timestamps.
        /// </summary>
        [SerializedAs(Name = "make_non_negative")]
        MakeNonNegative = 0,

        /// <summary>
        /// Shift timestamps so that the first timestamp is 0.
        /// </summary>
        [SerializedAs(Name = "make_zero")]
        MakeZero = 1,

        /// <summary>
        /// Enables shifting when required by the target format.
        /// </summary>
        [SerializedAs(Name = "auto")]
        Auto = 3,

        /// <summary>
        /// Disables shifting of timestamp.
        /// </summary>
        [SerializedAs(Name = "disabled")]
        Disabled = 4
    }
}
