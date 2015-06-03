using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Exceptions;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Command
{
    public class CommandOutput
    {
        private CommandOutput(IContainer outputToUse, SettingsCollection outputSettings, bool export)
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
            Id = Guid.NewGuid().ToString();
        }

        public static CommandOutput Create(IContainer outputToUse)
        {
            return Create(outputToUse, SettingsCollection.ForOutput());
        }
        public static CommandOutput Create(IContainer outputToUse, SettingsCollection outputSettings)
        {
            return Create(outputToUse, outputSettings, true);
        }
        public static CommandOutput Create(IContainer outputToUse, SettingsCollection outputSettings, bool export)
        {
            return new CommandOutput(outputToUse, outputSettings, export);
        }

        public bool IsExported { get; set; }
        
        public SettingsCollection Settings { get; set; }

        public IContainer Resource { get; internal set; }

        public string OutputName { get { return Resource.FullName; } }

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
