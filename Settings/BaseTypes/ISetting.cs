namespace Hudl.Ffmpeg.Settings.BaseTypes
{
    /// <summary>
    /// representation of a settomg that can be applied to an Ffmpeg resource
    /// </summary>
    public interface ISetting
    {
        /// <summary>
        /// the command name for the affect
        /// </summary>
        string Type { get; }

        /// <summary>
        /// builds the command necessary to complete the effect
        /// </summary>
        string ToString();
    }
}
