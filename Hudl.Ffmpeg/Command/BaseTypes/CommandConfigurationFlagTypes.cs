using System;

namespace Hudl.Ffmpeg.Command.BaseTypes
{
    /// <summary>
    /// Flags that turn on or off features in the render process
    /// </summary>
    [Flags]
    public enum CommandConfigurationFlagTypes
    {
        /// <summary>
        /// The render should perform normally with all defaults.
        /// </summary>
        None = 0, 

        /// <summary>
        /// If the render encounters the "Signal 15: Terminating" in a render, we will retry 1 time. 
        /// </summary>
        RetrySignal15Termination = 1
    }
}
