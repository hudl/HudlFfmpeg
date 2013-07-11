namespace Hudl.Ffmpeg.Common
{
    /// <summary>
    /// Enumeration that describes how to approach a merge of collections
    /// </summary>
    public enum FfmpegMergeOptionType
    {
        /// <summary>
        /// value indicates that a new value from a merge wins and replaces the option in the collection, new values are appended.
        /// </summary>
        NewWins = 0,
        /// <summary>
        /// value indicates that an existing value from a merge wins and only new values are appended in to the collection
        /// </summary>
        OldWins = 1,
    }
}
