using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Templates.BaseTypes
{
    /// <summary>
    /// This is the base template file for CommandFactory projects. This format will contain the necessary base functions for adding and assigning video, audio, and images.
    /// </summary>
    public abstract class BaseFilterchainAndSettingsTemplate
    {
        protected BaseFilterchainAndSettingsTemplate(IResource resourceToUse, SettingsCollectionResourceType collectionResourceType)
        {
            var resourceList = new List<IResource> { resourceToUse };
            BaseFilterchain = Filterchain.FilterTo(resourceList);
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
            }
        }

        public static implicit operator Filterchainv2(BaseFilterchainAndSettingsTemplate filterchainAndSettingsTemplate)
        {
            return filterchainAndSettingsTemplate.BaseFilterchain;
        }

        public static implicit operator SettingsCollection(BaseFilterchainAndSettingsTemplate filterchainAndSettingsTemplate)
        {
            return filterchainAndSettingsTemplate.BaseSettings;
        }

        #region Internals
        internal protected SettingsCollection BaseSettings { get; protected set; }
        internal protected Filterchainv2 BaseFilterchain { get; protected set; }
        #endregion
    }
}
