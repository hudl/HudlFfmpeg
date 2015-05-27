using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Sugar
{
    public static class CommandStageExtensions
    {
        public static CommandStage WithInput<TStreamType>(this CommandStage command, string fileName)
            where TStreamType : class, IStream

        {
            return command.WithInput<TStreamType>(fileName, SettingsCollection.ForInput());
        }
        public static CommandStage WithInput<TStreamType>(this CommandStage command, string fileName, SettingsCollection settings)
            where TStreamType : class, IStream
        {
            command.Command.AddInput(fileName, settings);

            return command.Select(command.Command.LastInputStream<TStreamType>());
        }
        public static CommandStage WithInput<TStreamType>(this CommandStage command, List<string> files)
            where TStreamType : class, IStream
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Files cannot be null or empty.", "files");
            }

            var streamIds = files.Select(fileName =>
            {
                command.Command.AddInput(fileName);

                return command.Command.LastInputStream<TStreamType>();
            }).ToList();

            return command.Select(streamIds);
        }
        public static CommandStage WithInputNoLoad(this CommandStage command, string fileName)
        {
            command.Command.AddInputNoLoad(fileName);

            return command.Select(command.Command.LastInputStream());
        }

        public static void ValidateStreams(CommandStage stage, List<StreamIdentifier> streamIds)
        {
            if (stage.Command.Owner == null)
            {
                throw new ArgumentException("Command must contain an owner before sugar is allowed.", "stage");
            }

            if (streamIds == null)
            {
                throw new ArgumentNullException("streamIds");
            }
        }
        public static CommandStage Select(this CommandStage stage, int index)
        {
            var streamId = stage.Command.StreamIdentifier(index);

            return stage.Select(streamId);
        }
        public static CommandStage Select<TStreamType>(this CommandStage stage, int index)
            where TStreamType : class, IStream
        {
            var streamId = stage.Command.StreamIdentifier<TStreamType>(index);

            return stage.Select(streamId);
        }
        public static CommandStage Select(this CommandStage stage, CommandInput resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            return stage.Select(resource.GetStreamIdentifier());
        }
        public static CommandStage Select<TStreamType>(this CommandStage stage, CommandInput resource)
            where TStreamType : class, IStream
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            return stage.Select(resource.GetStreamIdentifier<TStreamType>());
        }
        public static CommandStage Select(this CommandStage stage, StreamIdentifier streamId)
        {
            var streamIdList = new List<StreamIdentifier> { streamId };
            
            return stage.Select(streamIdList);
        }
        public static CommandStage Select(this CommandStage stage, params StreamIdentifier[] streamIds)
        {
            var streamIdList = new List<StreamIdentifier>(streamIds);

            return stage.Select(streamIdList);
        }
        public static CommandStage Select(this CommandStage stage, List<StreamIdentifier> streamIds)
        {
            ValidateStreams(stage, streamIds);

            stage.StreamIdentifiers.AddRange(streamIds);

            return stage;
        }

        public static CommandStage Take(this CommandStage stage, int index)
        {
            var streamId = stage.StreamIdentifiers[index];

            return stage.Command.Select(streamId);
        }

        public static void ValidateFilter(FFmpegCommand command, Filterchain filterchain)
        {
            if (command.Owner == null)
            {
                throw new ArgumentException("Command must contain an owner before sugar is allowed.", "command");
            }

            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }
        }
        public static CommandStage Filter(this CommandStage stage, Filterchain filterchain)
        {
            var outputStreamIdentifiers = stage.Command.FilterchainManager.Add(filterchain, stage.StreamIdentifiers.ToArray());

            return stage.Command.Select(outputStreamIdentifiers);
        }
        public static CommandStage Filter(this CommandStage stage, FilterchainTemplate filterchainTemplate)
        {
            var outputStreamIdentifiers = filterchainTemplate.SetupTemplate(stage.Command, stage.StreamIdentifiers);

            return stage.Command.Select(outputStreamIdentifiers); 
        }
        public static CommandStage FilterEach(this CommandStage stage, Filterchain filterchain)
        {
            var outputStreamIdentifiers = stage.Command.FilterchainManager.AddToEach(filterchain, stage.StreamIdentifiers.ToArray());

            return stage.Command.Select(outputStreamIdentifiers);
        }

        public static void ValidateMapTo(FFmpegCommand command)
        {
            if (command.Owner == null)
            {
                throw new ArgumentException("Command must contain an owner before sugar is allowed.", "command");
            }
        }
        public static List<CommandOutput> MapTo<TOutputType>(this CommandStage stage)
            where TOutputType : class, IContainer, new()
        {
            return stage.MapTo<TOutputType>(SettingsCollection.ForOutput());
        }
        public static List<CommandOutput> MapTo<TOutputType>(this CommandStage stage, SettingsCollection settings)
            where TOutputType : class, IContainer, new()
        {
            return stage.MapTo<TOutputType>(string.Empty, settings);
        }
        public static List<CommandOutput> MapTo<TOutputType>(this CommandStage stage, string fileName, SettingsCollection settings)
            where TOutputType : class, IContainer, new()
        {
            ValidateMapTo(stage.Command);

            var commandOutput = CommandHelper.SetupCommandOutputMaps<TOutputType>(stage, settings, fileName);

            stage.Command.OutputManager.Add(commandOutput);

            return new List<CommandOutput>
                {
                    commandOutput
                };
        }

        private static void ValidateTo(FFmpegCommand command)
        {
            if (command.Owner == null)
            {
                throw new ArgumentException("Command must contain an owner before sugar is allowed.", "command");
            }
        }
        public static List<CommandOutput> To<TOutputType>(this CommandStage stage)
           where TOutputType : class, IContainer, new()
        {
            return stage.To<TOutputType>(SettingsCollection.ForOutput());
        }
        public static List<CommandOutput> To<TOutputType>(this CommandStage stage, SettingsCollection settings)
            where TOutputType : class, IContainer, new()
        {
            return stage.To<TOutputType>(string.Empty, settings);
        }
        public static List<CommandOutput> To<TOutputType>(this CommandStage stage, string fileName, SettingsCollection settings)
            where TOutputType : class, IContainer, new()
        {
            ValidateTo(stage.Command);

            var commandOutput = CommandHelper.SetupCommandOutput<TOutputType>(stage.Command, settings, fileName);

            stage.Command.OutputManager.Add(commandOutput);

            return new List<CommandOutput>
                {
                    commandOutput
                };    
        }
        
        public static CommandStage BeforeRender(this CommandStage command, Action<ICommandFactory, ICommand, bool> action)
        {
            command.Command.PreExecutionAction = action;

            return command; 
        }
        public static CommandStage AfterRender(this CommandStage command, Action<ICommandFactory, ICommand, bool> action)
        {
            command.Command.PostExecutionAction = action;

            return command;
        }
    }
}