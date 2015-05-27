using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Exceptions;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Command
{
    /// <summary>
    /// Represents a single resource file for a command. 
    /// </summary>
    public class CommandInput
    {
        private CommandInput(IContainer resource, SettingsCollection settings)
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
        
        public static CommandInput Create(IContainer resource)
        {
            return Create(resource, SettingsCollection.ForInput());
        }

        public static CommandInput Create(IContainer resource, SettingsCollection settings)
        {
            return new CommandInput(resource, settings);
        }

        /// <summary>
        /// the resource input file that is part of the command.
        /// </summary>
        public IContainer Resource { get; set; }

        /// <summary>
        /// the collection of settings that apply to this input
        /// </summary>
        public SettingsCollection Settings { get; set; }

        public StreamIdentifier GetStreamIdentifier()
        {
            if (Resource.Streams.OfType<VideoStream>().Any())
            {
                return GetStreamIdentifier<VideoStream>();
            }

            if (Resource.Streams.OfType<AudioStream>().Any())
            {
                return GetStreamIdentifier<AudioStream>();
            }

            throw new StreamNotFoundException();
        }

        public StreamIdentifier GetStreamIdentifier<TStreamType>()
            where TStreamType : class, IStream
        {
            var streamInReference = Resource.Streams.OfType<TStreamType>().FirstOrDefault();

            if (streamInReference == null)
            {
                throw new StreamNotFoundException(typeof(TStreamType));
            }

            return StreamIdentifier.Create(Owner.Owner.Id, Owner.Id, streamInReference.Map);
        }

        public List<StreamIdentifier> GetStreamIdentifiers()
        {
            return Resource.Streams
                           .Select(s => StreamIdentifier.Create(Owner.Owner.Id, Owner.Id, s.Map))
                           .ToList();
        } 

        #region Internals
        internal string Id { get; set; }
        internal FFmpegCommand Owner { get; set; }
        #endregion
    }
}
