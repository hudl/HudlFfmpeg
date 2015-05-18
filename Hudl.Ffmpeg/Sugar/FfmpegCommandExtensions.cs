using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Sugar
{
    public static class FFmpegCommandExtensions
    {
        public static void ValidateInput(FFmpegCommand command, string fileName)
        {
            if (command.Owner == null)
            {
                throw new ArgumentException("Command must contain an owner before sugar is allowed.", "command");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Command fileName cannot be null or empty.", "fileName");
            }

            //TODO: fix
            //if (command.Owner.CommandList.All(c => c.Id != command.Id))
            //{
            //    throw new ArgumentException("Command must be added via CreateOutput or CreateResource first.", "command");
            //}
        }
        public static FFmpegCommand AddInput(this FFmpegCommand command, string fileName)
        {
            return command.AddInput(fileName, SettingsCollection.ForInput());
        }
        public static FFmpegCommand AddInput(this FFmpegCommand command, string fileName, SettingsCollection settings)
        {
            ValidateInput(command, fileName);

            var resource = Resource.From(fileName)
                                   .LoadMetadata();

            var commandResource = CommandInput.Create(resource, settings);

            command.InputManager.Add(commandResource);

            return command;
        }
        public static FFmpegCommand AddInput(this FFmpegCommand command, List<string> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Files cannot be null or empty.", "files");
            }

            files.ForEach(fileName => command.AddInput(fileName));

            return command;
        }
        public static FFmpegCommand AddInputNoLoad(this FFmpegCommand command, string fileName)
        {
            ValidateInput(command, fileName);

            var resource = Resource.From(fileName);

            var commandResource = CommandInput.Create(resource);

            command.InputManager.Add(commandResource);

            return command;
        }

        //stream ids
        public static StreamIdentifier StreamIdentifier(this FFmpegCommand command, int index)
        {
            if (index < 0 || index >= command.Inputs.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return command.Inputs[index].GetStreamIdentifier();
        }
        public static StreamIdentifier StreamIdentifier<TStreamType>(this FFmpegCommand command, int index)
            where TStreamType : class, IStream
        {
            if (index < 0 || index >= command.Inputs.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return command.Inputs[index].GetStreamIdentifier<TStreamType>();
        }

        public static StreamIdentifier LastInputStream(this FFmpegCommand command)
        {
            if (command.Inputs.Count == 0)
            {
                return null;
            }

            return command.Inputs[command.Inputs.Count - 1].GetStreamIdentifier();
        }
        public static StreamIdentifier LastInputStream<TStreamType>(this FFmpegCommand command)
            where TStreamType : class, IStream
        {
            if (command.Inputs.Count == 0)
            {
                return null;
            }

            return command.Inputs[command.Inputs.Count - 1].GetStreamIdentifier<TStreamType>();
        }

        //command output lists
        private static void ValidateTo(FFmpegCommand command)
        {
            if (command.Owner == null)
            {
                throw new ArgumentException("Command must contain an owner before sugar is allowed.", "command");
            }
        }
        public static List<CommandOutput> To<TOutputType>(this FFmpegCommand command)
           where TOutputType : class, IContainer, new()
        {
            return command.To<TOutputType>(SettingsCollection.ForOutput());
        }
        public static List<CommandOutput> To<TOutputType>(this FFmpegCommand command, SettingsCollection settings)
            where TOutputType : class, IContainer, new()
        {
            return command.To<TOutputType>(string.Empty, settings);
        }
        public static List<CommandOutput> To<TOutputType>(this FFmpegCommand command, string fileName, SettingsCollection settings)
            where TOutputType : class, IContainer, new()
        {
            ValidateTo(command);

            var commandOutput = CommandHelper.SetupCommandOutput<TOutputType>(command, settings, fileName);

            command.OutputManager.Add(commandOutput);

            return new List<CommandOutput>
                {
                    commandOutput
                };
        }

        //command stage 
        public static CommandStage WithInput<TStreamType>(this FFmpegCommand command, string fileName)
            where TStreamType : class, IStream
        {
            var commandStage = CommandStage.Create(command);

            return commandStage.WithInput<TStreamType>(fileName); 
        }
        public static CommandStage WithInput<TStreamType>(this FFmpegCommand command, string fileName, SettingsCollection settings)
            where TStreamType : class, IStream
        {
            command.AddInput(fileName, settings);

            return command.Select(command.LastInputStream<TStreamType>());
        }
        public static CommandStage WithInput<TStreamType>(this FFmpegCommand command, List<string> files)
            where TStreamType : class, IStream
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Files cannot be null or empty.", "files");
            }

            var streamIds = files.Select(fileName =>
                {
                    command.AddInput(fileName);

                    return command.LastInputStream<TStreamType>();
                }).ToList();

            return command.Select(streamIds);
        }
        public static CommandStage WithInputNoLoad(this FFmpegCommand command, string fileName)
        {
            command.AddInputNoLoad(fileName);

            return command.Select(command.LastInputStream());
        }
        
        private static void ValidateStreams(FFmpegCommand command, List<StreamIdentifier> streamIds)
        {
            if (command.Owner == null)
            {
                throw new ArgumentException("Command must contain an owner before sugar is allowed.", "command");
            }

            if (streamIds == null)
            {
                throw new ArgumentNullException("streamIds");
            }
        }
        public static CommandStage Select(this FFmpegCommand command, int index)
        {
            var streamId = command.StreamIdentifier(index);

            return command.Select(streamId);
        }
        public static CommandStage Select<TStreamType>(this FFmpegCommand command, int index)
            where TStreamType : class, IStream
        {
            var streamId = command.StreamIdentifier<TStreamType>(index);

            return command.Select(streamId); 
        }
        public static CommandStage Select(this FFmpegCommand command, CommandInput resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            return command.Select(resource.GetStreamIdentifier());
        }
        public static CommandStage Select<TStreamType>(this FFmpegCommand command, CommandInput resource)
            where TStreamType : class, IStream
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            return command.Select(resource.GetStreamIdentifier<TStreamType>());
        }
        public static CommandStage Select(this FFmpegCommand command, StreamIdentifier streamId)
        {
            var streamIdList = new List<StreamIdentifier>() { streamId };
            
            return command.Select(streamIdList);
        }
        public static CommandStage Select(this FFmpegCommand command, params StreamIdentifier[] streamIds)
        {
            var streamIdList = new List<StreamIdentifier>(streamIds);
            
            return command.Select(streamIdList);
        }
        public static CommandStage Select(this FFmpegCommand command, List<StreamIdentifier> streamIds)
        {
            ValidateStreams(command, streamIds);

            return new CommandStage(command)
            {
                StreamIdentifiers = streamIds
            };
        }
        public static CommandStage SelectAll(this FFmpegCommand command)
        {
            var streamIdList = command.Inputs.SelectMany(r => r.GetStreamIdentifiers()).ToList();

            return command.Select(streamIdList);
        }
    }
}
