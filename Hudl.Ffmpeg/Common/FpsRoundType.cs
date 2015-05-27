namespace Hudl.FFmpeg.Common
{
    /// <summary>
    /// enumaration containing the rounding functions for the fps filter
    /// </summary>
    /// <remarks></remarks>
    public enum FpsRoundType
    {
        /// <summary>
        /// round to the nearest
        /// </summary>
        Near,
        /// <summary>
        /// round toward +infinity
        /// </summary>
        Up,
        /// <summary>
        /// round toward -infinity
        /// </summary>
        Down,
        /// <summary>
        /// round away from 0
        /// </summary>
        Inf,
        /// <summary>
        /// round towards 0
        /// </summary>
        Zero
    }
}
