using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Settings.Interfaces;
using Hudl.FFmpeg.Settings.Utility;

namespace Hudl.FFmpeg.Settings.BaseTypes
{
    /// <summary>
    /// A series of Settings that apply to a single resource type.
    /// </summary>
    public class SettingsCollection
    {
        internal SettingsCollection()
            : this(SettingsCollectionResourceType.Input)
        {
        }
        internal SettingsCollection(params ISetting[] settings)
            : this(SettingsCollectionResourceType.Input, settings)
        {
        }
        internal SettingsCollection(SettingsCollectionResourceType type, params ISetting[] settings)
        {
            Type = type;
            SettingsList = new List<ISetting>();
            if (settings.Length > 0)
            {
                new List<ISetting>(settings).ForEach(s => Add(s));
            }
        }

        public ReadOnlyCollection<ISetting> Items { get { return SettingsList.AsReadOnly(); } }

        public SettingsCollectionResourceType Type { get; protected set; }

        public int Count { get { return SettingsList.Count;  } }

        /// <summary>
        /// returns a new settings collection instance for input collections
        /// </summary>
        public static SettingsCollection ForInput(params ISetting[] settings)
        {
            return new SettingsCollection(SettingsCollectionResourceType.Input, settings);
        }

        /// <summary>
        /// returns a new settings collection instance for output collections
        /// </summary>
        public static SettingsCollection ForOutput(params ISetting[] settings)
        {
            return new SettingsCollection(SettingsCollectionResourceType.Output, settings);
        }

        /// <summary>
        /// adds the given Setting to the SettingsCollection
        /// </summary>
        /// <typeparam name="TSetting">the generic type of the Setting</typeparam>
        /// <param name="setting">the Setting to be added to the SettingsCollection</param>
        public SettingsCollection Add<TSetting>(TSetting setting)
            where TSetting : ISetting
        {
            if (setting == null)
            {
                throw new ArgumentNullException("setting");
            }
            if (Contains(setting))
            {
                throw new ArgumentException(string.Format("The SettingsCollection already contains a type of {0}.", setting.GetType().Name), "setting");
            }
            if (!Validate.IsSettingFor(setting, Type))
            {
                throw new ArgumentException(string.Format("The SettingsCollection is restricted only to {0} settings.", Type), "setting");
            }

            SettingsList.Add(setting);
            return this;
        }

        /// <summary>
        /// adds the given SettingsCollection range to the SettingsCollection
        /// </summary>
        /// <param name="settings">the SettingsCollection to be added  to be added to the SettingsCollection</param>
        public SettingsCollection AddRange(SettingsCollection settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.SettingsList.ForEach(s => Add(s));
            return this;
        }

        /// <summary>
        /// merges the current setting into the set based on the merge option type
        /// </summary>
        public SettingsCollection Merge<TSetting>(TSetting setting, FFmpegMergeOptionType option)
            where TSetting : ISetting
        {
            if (setting == null)
            {
                throw new ArgumentNullException("setting");
            }

            var alreadyContainsSetting = Contains(setting); 
            if (alreadyContainsSetting)
            {
                if (option == FFmpegMergeOptionType.NewWins)
                {
                    Remove(setting);
                    Add(setting);
                }
            }
            else
            {
                Add(setting);
            }

            return this;
        }

        /// <summary>
        /// merges the current SettingsCollection into the set based on the merge option type.
        /// </summary>
        public SettingsCollection MergeRange(SettingsCollection settings, FFmpegMergeOptionType option)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (settings.Type != Type)
            {
                throw new ArgumentException(string.Format("Settings parameter must be of the same type {0} as the SettingsCollection.", Type));
            }

            settings.SettingsList.ForEach(s => Merge(s, option));
            return this;
        }
        
        /// <summary>
        /// determines if the settings collection already contains this setting type
        /// </summary>
        public bool Contains<TSetting>()
            where TSetting : ISetting
        {
            return SettingsList.Any(s => s is TSetting);
        }

        /// <summary>
        /// determines if the settings collection already contains this setting type
        /// </summary>
        public bool Contains<TSetting>(TSetting item)
            where TSetting : ISetting
        {
            var itemType = item.GetType();

            return !item.IsMultipleAllowed() && SettingsList.Any(s => s.GetType().IsAssignableFrom(itemType));
        }

        /// <summary>
        /// will return the TSetting item in the settings collection list
        /// </summary>
        public TSetting Item<TSetting>()
            where TSetting : class, ISetting
        {
            return SettingsList.OfType<TSetting>().FirstOrDefault();
        }

        /// <summary>
        /// will return the List of ISetting that match the type provided
        /// </summary>
        public List<TSetting> OfType<TSetting>()
            where TSetting : class, ISetting
        {
            return SettingsList.OfType<TSetting>().ToList();
        }

        /// <summary>
        /// will return an exact copy of the settings collection.
        /// </summary>
        public SettingsCollection Copy()
        {
            var newSettingsCollection = new SettingsCollection(Type);
            newSettingsCollection.MergeRange(this, FFmpegMergeOptionType.NewWins);
            return newSettingsCollection;
        }

        /// <summary>
        /// removes the specified setting type from the SettingsCollection
        /// </summary>
        /// <typeparam name="TSetting">the settings type that is to be removed</typeparam>
        public SettingsCollection Remove<TSetting>()
            where TSetting : ISetting
        {
            SettingsList.RemoveAll(s => s is TSetting);
            return this;
        }

        /// <summary>
        /// removes the specified setting type from the SettingsCollection
        /// </summary>
        /// <typeparam name="TSetting">the settings type that is to be removed</typeparam>
        public SettingsCollection Remove<TSetting>(TSetting setting)
            where TSetting : ISetting
        {
            var settingType = setting.GetType();
            SettingsList.RemoveAll(s => s.GetType().IsAssignableFrom(settingType));
            return this;
        }

        /// <summary>
        /// removes the Setting at the given index from the SettingsCollection
        /// </summary>
        /// <param name="index">the index of the desired Setting to be removed from the SettingsCollection</param>
        public SettingsCollection RemoveAt(int index)
        {
            SettingsList.RemoveAt(index);
            return this;
        }

        /// <summary>
        /// removes all the Setting matching the provided criteria
        /// </summary>
        /// <param name="pred">the predicate of required criteria</param>
        public SettingsCollection RemoveAll(Predicate<ISetting> pred)
        {
            SettingsList.RemoveAll(pred);
            return this;
        }

        #region Internals
        internal List<ISetting> SettingsList { get; set; }
        #endregion 
    }
}
