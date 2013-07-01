using System;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class CommandOutput<TResource>
        where TResource : IResource
    {
        internal CommandOutput(Command<TResource> parent, TResource resource)
            : this(parent, SettingsCollection.ForOutput(), resource)
        {
        }
        internal CommandOutput(Command<TResource> parent, SettingsCollection settings, TResource resource)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            if (settings.Type != SettingsCollectionResourceTypes.Output)
            {
                throw new ArgumentException("CommandOutput only accepts output settings collections");
            }

            Parent = parent;
            Resource = resource;
            Settings = settings; 
        }

        public Command<TResource> Parent { get; protected set; }

        public SettingsCollection Settings { get; set; }

        public TimeSpan Length
        {
            get
            {
                return TimeSpan.FromSeconds(Helpers.GetLength(Parent));
            }
        }

        public TResource Output()
        {
            Resource.Length = Length;
            return Resource;
        }

        #region Internals
        internal TResource Resource { get; set; }
        #endregion 
    }
}
