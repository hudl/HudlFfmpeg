using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class Command
    {
        /// <summary>
        /// Creates a new command using TOutput as a new instance
        /// </summary>
        /// <param name="parent">The command factory that the command will belong to.</param>
        public static Command<TOutput> OutputTo<TOutput>(CommandFactory parent)
            where TOutput : IResource, new()
        {
            return OutputTo(parent, new TOutput(), true);
        }

        /// <summary>
        /// Creates a new command using TOutput as a new instance
        /// </summary>
        /// <param name="parent">The command factory that the command will belong to.</param>
        /// <param name="export">Determines if the output from this command should be included in factory output</param>
        public static Command<TOutput> OutputTo<TOutput>(CommandFactory parent, bool export)
            where TOutput : IResource, new()
        {
            return OutputTo(parent, new TOutput(), export);
        }

        /// <summary>
        /// Creates a new command using outputToUse as definition for the output file
        /// </summary>
        /// <param name="parent">The command factory that the command will belong to.</param>
        /// <param name="outputToUse">The output definition to use in the ffmpeg command.</param>
        public static Command<TOutput> OutputTo<TOutput>(CommandFactory parent, TOutput outputToUse)
            where TOutput : IResource, new()
        {
            return OutputTo(parent, outputToUse, true);
        }

        /// <summary>
        /// Creates a new command using outputToUse as definition for the output file
        /// </summary>
        /// <param name="parent">The command factory that the command will belong to.</param>
        /// <param name="outputToUse">The output definition to use in the ffmpeg command.</param>
        /// <param name="export">Determines if the output from this command should be included in factory output</param>
        public static Command<TOutput> OutputTo<TOutput>(CommandFactory parent, TOutput outputToUse, bool export)
            where TOutput : IResource, new()
        {
            return OutputTo(parent, outputToUse, SettingsCollection.ForOutput(), export);
        }

        /// <summary>
        /// Creates a new command using outputToUse as definition for the output file
        /// </summary>
        /// <param name="parent">The command factory that the command will belong to.</param>
        /// <param name="outputToUse">The output definition to use in the ffmpeg command.</param>
        /// <param name="outputSettings">The output settings to use for the command.</param>
        public static Command<TOutput> OutputTo<TOutput>(CommandFactory parent, TOutput outputToUse, SettingsCollection outputSettings)
            where TOutput : IResource, new()
        {
            return OutputTo(parent, outputToUse, SettingsCollection.ForOutput(), true);
        }

        /// <summary>
        /// Creates a new command using outputToUse as definition for the output file
        /// </summary>
        /// <param name="parent">The command factory that the command will belong to.</param>
        /// <param name="outputToUse">The output definition to use in the ffmpeg command.</param>
        /// <param name="outputSettings">The output settings to use for the command.</param>
        /// <param name="export">Determines if the output from this command should be included in factory output</param>
        public static Command<TOutput> OutputTo<TOutput>(CommandFactory parent, TOutput outputToUse, SettingsCollection outputSettings, bool export)
            where TOutput : IResource, new()
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            if (outputToUse == null)
            { 
                throw new ArgumentNullException("outputToUse");
            }
            if (outputSettings == null)
            {
                throw new ArgumentNullException("outputSettings");
            }

            return new Command<TOutput>(parent, outputToUse, outputSettings, export); 
        }
    }
}
