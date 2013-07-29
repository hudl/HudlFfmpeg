using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Filters.Templates;
using Hudl.Ffmpeg.Resolution;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Templates.BaseTypes
{
    /// <summary>
    /// This is the base template file for CommandFactory projects. This format will contain the necessary base functions for adding and assigning video, audio, and images.
    /// </summary>
    public abstract class BaseFilterchainAndSettingsTemplate<TOutput>
        where TOutput : IResource, new()
    {
        protected BaseFilterchainAndSettingsTemplate(SettingsCollectionResourceType collectionResourceType)
        {
            BaseFilterchain = Filterchain.FilterTo<TOutput>();
            switch (collectionResourceType)
            {
                case SettingsCollectionResourceType.Input:
                    BaseSettings = SettingsCollection.ForInput();
                    break;
                case SettingsCollectionResourceType.Output:
                    BaseSettings = SettingsCollection.ForOutput();
                    break;
                default: 
                    throw new InvalidOperationException("Cannot add a setting template type of Any");
                    break;
            }
        }

        public static implicit operator Filterchain<TOutput>(BaseFilterchainAndSettingsTemplate<TOutput> filterchainAndSettingsTemplate)
        {
            return filterchainAndSettingsTemplate.BaseFilterchain;
        }

        public static implicit operator Filterchain<IResource>(BaseFilterchainAndSettingsTemplate<TOutput> filterchainAndSettingsTemplate)
        {
            return filterchainAndSettingsTemplate.BaseFilterchain;
        }

        public static implicit operator SettingsCollection(BaseFilterchainAndSettingsTemplate<TOutput> filterchainAndSettingsTemplate)
        {
            return filterchainAndSettingsTemplate.BaseSettings;
        }

        #region Internals
        internal protected SettingsCollection BaseSettings { get; protected set; }
        internal protected Filterchain<TOutput> BaseFilterchain { get; protected set; }
        #endregion
    }
}
