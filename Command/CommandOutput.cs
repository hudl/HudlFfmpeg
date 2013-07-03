using System;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class CommandOutput<TResource>
        where TResource : IResource
    {
        internal CommandOutput(Command<TResource> parent, TResource outputToUse)
            : this(parent, outputToUse, SettingsCollection.ForOutput(), true)
        {
        }
        internal CommandOutput(Command<TResource> parent, TResource outputToUse, SettingsCollection outputSettings)
            : this(parent, outputToUse, outputSettings, true)
        {
        }
        internal CommandOutput(Command<TResource> parent, TResource outputToUse, SettingsCollection outputSettings, bool export)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            if (outputSettings == null)
            {
                throw new ArgumentNullException("outputSettings");
            }
            if (outputToUse == null)
            {
                throw new ArgumentNullException("outputToUse");
            }
            if (outputSettings.Type != SettingsCollectionResourceTypes.Output)
            {
                throw new ArgumentException("CommandOutput only accepts output settings collections");
            }

            Parent = parent;
            Resource = outputToUse;
            Settings = outputSettings;
            IsExported = export;
        }

        public Command<TResource> Parent { get; protected set; }

        public SettingsCollection Settings { get; set; }

        public bool IsExported { get; set; }

        public TimeSpan Length
        {
            get
            {
                return TimeSpan.FromSeconds(Helpers.GetLength(Parent as Command<IResource>));
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
