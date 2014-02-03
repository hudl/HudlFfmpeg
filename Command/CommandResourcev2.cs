using System;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    /// <summary>
    /// Represents a single resource file for a command. 
    /// </summary>
    public class CommandResourcev2
    {
        private CommandResourcev2(IResource resource, SettingsCollection settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            if (settings.Type != SettingsCollectionResourceType.Input)
            {
                throw new ArgumentException("CommandResource only accepts input settings collections");
            }

            Resource = resource;
            Settings = settings;
            Id = Guid.NewGuid().ToString();
        }
        
        public static CommandResourcev2 Create(IResource resource)
        {
            return Create(resource, SettingsCollection.ForInput());
        }

        public static CommandResourcev2 Create(IResource resource, SettingsCollection settings)
        {
            return new CommandResourcev2(resource, settings);
        }

        /// <summary>
        /// the resource input file that is part of the command.
        /// </summary>
        public IResource Resource { get; set; }

        /// <summary>
        /// the collection of settings that apply to this input
        /// </summary>
        public SettingsCollection Settings { get; set; }

        /// <summary>
        /// returns a receipt for the command resource
        /// </summary>
        /// <returns></returns>
        public CommandReceipt GetReceipt()
        {
            return CommandReceipt.CreateFromInput(Owner.Owner.Id, Owner.Id, Resource.Map);
        }

        #region Internals
        internal string Id { get; set; }
        internal Commandv2 Owner { get; set; }
        #endregion
    }
}
