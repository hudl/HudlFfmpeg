namespace Hudl.FFmpeg.Settings.BaseTypes
{
    /// <summary>
    /// representation of a settomg that can be applied to an FFmpeg resource
    /// </summary>
    public interface ISetting
    {
        /// <summary>
        /// the command name for the affect
        /// </summary>
        string Type { get; }

        /// <summary>
        /// A setting must contain a validation system for stringification
        /// </summary>
        void Validate();
        
        /// <summary>
        /// builds the command necessary to complete the effect
        /// </summary>
        string GetAndValidateString(); 
    }
}
