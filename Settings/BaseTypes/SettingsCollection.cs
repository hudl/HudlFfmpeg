using System;
using System.Collections.Generic;

namespace Hudl.Ffmpeg.Settings.BaseTypes
{
    /// <summary>
    /// A series of Settings that apply to a single resource type.
    /// </summary>
    public class SettingsCollection
    {
        public SettingsCollection()
        {
            _settings = new List<ISetting>();
        }
        public SettingsCollection(params ISetting[] settings)
        {
            _settings = new List<ISetting>(settings);
        }

        private readonly List<ISetting> _settings;
        public IReadOnlyList<ISetting> Items { get { return _settings.AsReadOnly(); } }

        /// <summary>
        /// adds the given Setting to the SettingsCollection
        /// </summary>
        /// <typeparam name="TSetting">the generic type of the Setting</typeparam>
        /// <param name="setting">the Setting to be added to the SettingsCollection</param>
        public SettingsCollection Add<TSetting>(TSetting setting)
            where TSetting : ISetting
        {
            _settings.Add(setting);
            return this;
        }

        /// <summary>
        /// adds the given SettingsCollection range to the SettingsCollection
        /// </summary>
        /// <param name="settings">the SettingsCollection to be added  to be added to the SettingsCollection</param>
        public SettingsCollection AddRange(SettingsCollection settings)
        {
            _settings.AddRange(settings.Items);
            return this;
        }

        /// <summary>
        /// removes the Setting at the given index from the SettingsCollection
        /// </summary>
        /// <param name="index">the index of the desired Setting to be removed from the SettingsCollection</param>
        public SettingsCollection Remove(int index)
        {
            _settings.RemoveAt(index);
            return this;
        }

        /// <summary>
        /// removes all the Setting matching the provided criteria
        /// </summary>
        /// <param name="pred">the predicate of required criteria</param>
        public SettingsCollection RemoveAll(Predicate<ISetting> pred)
        {
            _settings.RemoveAll(pred);
            return this;
        }

    }
}
