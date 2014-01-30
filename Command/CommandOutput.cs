using System;
using System.IO;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class CommandOutput
    {
        private CommandOutput(IResource outputToUse, SettingsCollection outputSettings, bool export)
        {
            if (outputSettings == null)
            {
                throw new ArgumentNullException("outputSettings");
            }
            if (outputToUse == null)
            {
                throw new ArgumentNullException("outputToUse");
            }
            if (outputSettings.Type != SettingsCollectionResourceType.Output)
            {
                throw new ArgumentException("CommandOutput only accepts output settings collections");
            }

            Resource = outputToUse;
            Settings = outputSettings;
            IsExported = export;
        }

        public static CommandOutput Create(IResource outputToUse)
        {
            return Create(outputToUse, SettingsCollection.ForOutput());
        }
        public static CommandOutput Create(IResource outputToUse, SettingsCollection outputSettings)
        {
            return Create(outputToUse, outputSettings, true);
        }
        public static CommandOutput Create(IResource outputToUse, SettingsCollection outputSettings, bool export)
        {
            return new CommandOutput(outputToUse, outputSettings, export);
        }

        public bool IsExported { get; set; }
        
        public SettingsCollection Settings { get; set; }
        
        public TimeSpan Length
        {
            get
            {
                return TimeSpan.FromSeconds(Helpers.GetLength(Owner));
            }
        }

        public IResource Output()
        {
            Resource.Length = Length;
            return Resource;
        }

        /// <summary>
        /// returns a receipt for the command output
        /// </summary>
        /// <returns></returns>
        public CommandReceipt GetReceipt()
        {
            return CommandReceipt.CreateFromOutput(Owner.Owner.Id, Owner.Id, Resource.Map);
        }

        #region Internals
        internal Commandv2 Owner { get; set; }
        internal IResource Resource { get; set; }
        #endregion 
    }
}
