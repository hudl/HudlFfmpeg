using System;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    /// <summary>
    /// Represents a single resource file for a command. 
    /// </summary>
    /// <typeparam name="TResource">The resource type</typeparam>
    public class CommandResource<TResource>
        where TResource : IResource
    {
        internal CommandResource(Command<IResource> parent, TResource resource)
            : this(parent, SettingsCollection.ForInput(), resource)
        {
        }
        internal CommandResource(Command<IResource> parent, SettingsCollection settings, TResource resource)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            if (settings.Type != SettingsCollectionResourceTypes.Input)
            {
                throw new ArgumentException("CommandResource only accepts input settings collections");
            }

            Parent = parent;
            Resource = resource;
            Settings = settings;
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// the resource input file that is part of the command.
        /// </summary>
        public TResource Resource { get; set; }

        /// <summary>
        /// the collection of settings that apply to this input
        /// </summary>
        public SettingsCollection Settings { get; set; }

        /// <summary>
        /// returns a receipt for the command resource
        /// </summary>
        /// <returns></returns>
        public CommandResourceReceipt GetReciept()
        {
            return new CommandResourceReceipt(Parent.Parent.Id, Parent.Id, Resource.Map);
        }

        #region Internals
        internal string Id { get; set; }
        internal Command<IResource> Parent { get; set; }
        #endregion
    }
}
