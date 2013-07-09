using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Resources.BaseTypes;

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

        ///// <summary>
        ///// the difference the setting will make on the output video length
        ///// </summary>
        ///// <returns>Null indicates that the length difference does not apply</returns>
        //TimeSpan? LengthDifference { get; }

        ///// <summary>
        ///// the length override property is an override to the input video length
        ///// </summary>
        ///// <returns>Null indicates that the length difference does not apply</returns>
        //TimeSpan? LengthOverride { get; }

        /// <summary>
        /// the length override function, overrided when a setting requires a length change of output calculated from the resources.
        /// </summary>
        /// <returns>Null indicates that the length difference does not apply</returns>
        TimeSpan? LengthFromInputs(List<CommandResource<IResource>> resources);

        /// <summary>
        /// builds the command necessary to complete the effect
        /// </summary>
        string ToString();
    }
}
