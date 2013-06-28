namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// Basic values that define how duration of output should be measured in comparison to the inputs
    /// </summary>
    public enum DurationTypes
    {
        /// <summary>
        /// Indicates that the longest input duration wins.
        /// </summary>
        Longest = 0,
        /// <summary>
        /// Indicates that the shortest input duration wins.
        /// </summary>
        Shortest = 1,
        /// <summary>
        /// Indicates that the first intput resource duration wins.
        /// </summary>
        First = 2
    }
}
