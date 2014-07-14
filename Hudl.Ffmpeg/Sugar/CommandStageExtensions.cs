using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;
using Hudl.Ffmpeg.Resources;

namespace Hudl.Ffmpeg.Sugar
{
    public static class CommandStageExtensions
    {
        public static CommandStage WithInput(this CommandStage command, string fileName)
        {
            return command.WithInput(fileName, SettingsCollection.ForInput());
        }
        public static CommandStage WithInput(this CommandStage command, string fileName, SettingsCollection settings)
        {
            command.Command.AddInput(fileName, settings);

            return command.WithStreams(command.Command.LastInputReceipt());
        }
        public static CommandStage WithInput(this CommandStage command, List<string> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Files cannot be null or empty.", "files");
            }

            var receipts = files.Select(fileName =>
            {
                command.Command.AddInput(fileName);

                return command.Command.LastInputReceipt();
            }).ToList();

            return command.WithStreams(receipts);
        }
        public static CommandStage WithInput(this CommandStage command, List<CommandOutput> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Files cannot be null or empty.", "files");
            }

            var receipts = files.Select(co =>
            {
                command.Command.AddInputNoLoad(co.OutputName);

                return command.Command.LastInputReceipt();
            }).ToList();

            return command.WithStreams(receipts);
        }
        public static CommandStage WithInputNoLoad(this CommandStage command, string fileName)
        {
            command.Command.AddInputNoLoad(fileName);

            return command.WithStreams(command.Command.LastInputReceipt());
        }

        public static void ValidateRecipts(CommandStage stage, List<CommandReceipt> receipts)
        {
            if (stage.Command.Owner == null)
            {
                throw new ArgumentException("Command must contain an owner before sugar is allowed.", "stage");
            }

            if (receipts == null)
            {
                throw new ArgumentNullException("receipts");
            }
        }
        public static CommandStage WithStream(this CommandStage stage, CommandReceipt receipt)
        {
            var receiptList = new List<CommandReceipt> { receipt };
            return stage.WithStreams(receiptList);
        }
        public static CommandStage WithStreams(this CommandStage stage, params CommandReceipt[] receipts)
        {
            var receiptList = new List<CommandReceipt>(receipts);
            return stage.WithStreams(receiptList);
        }
        public static CommandStage WithStreams(this CommandStage stage, List<CommandReceipt> receipts)
        {
            ValidateRecipts(stage, receipts);

            stage.Receipts.AddRange(receipts);

            return stage;
        }
        public static CommandStage TakeStreamAt(this CommandStage stage, int index)
        {
            var receipt = stage.Receipts[index];

            return stage.Command.WithStreams(receipt); 
        }

        public static void ValidateFilter(FfmpegCommand command, Filterchain filterchain)
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
            var outputReceipts = stage.Command.FilterchainManager.Add(filterchain, stage.Receipts.ToArray());

            return stage.Command.WithStreams(outputReceipts);
        }
        public static CommandStage Filter(this CommandStage stage, FilterchainTemplate filterchainTemplate)
        {
            var outputReceipts = filterchainTemplate.SetupTemplate(stage.Command, stage.Receipts);

            return stage.Command.WithStreams(outputReceipts); 
        }
        public static CommandStage FilterEach(this CommandStage stage, Filterchain filterchain)
        {
            var outputReceipts = stage.Command.FilterchainManager.AddToEach(filterchain, stage.Receipts.ToArray());

            return stage.Command.WithStreams(outputReceipts);
        }

        public static CommandStage Copy(this CommandStage stage)
        {
            return new CommandStage(stage.Command)
                {
                    Receipts = stage.Receipts
                };
        }

        public static void ValidateMapTo(FfmpegCommand command)
        {
            if (command.Owner == null)
            {
                throw new ArgumentException("Command must contain an owner before sugar is allowed.", "command");
            }
        }
        public static List<CommandOutput> MapTo<TOutputType>(this CommandStage stage)
            where TOutputType : class, IResource, new()
        {
            return stage.MapTo<TOutputType>(SettingsCollection.ForOutput());
        }
        public static List<CommandOutput> MapTo<TOutputType>(this CommandStage stage, SettingsCollection settings)
            where TOutputType : class, IResource, new()
        {
            return stage.MapTo<TOutputType>(string.Empty, settings);
        }
        public static List<CommandOutput> MapTo<TOutputType>(this CommandStage stage, string fileName, SettingsCollection settings)
            where TOutputType : class, IResource, new()
        {
            ValidateMapTo(stage.Command);

            var settingsCopy = settings.Copy();
            var outputObjects = new List<CommandOutput>();
            var commandOutput = CommandOutput.Create(Resource.CreateOutput<TOutputType>(), settingsCopy);

            stage.Receipts.ForEach(receipt =>
            {
                var indexOfResource = stage.Command.Objects.Inputs.FindIndex(r => r.Resource.Map == receipt.Map);
                if (indexOfResource < 0)
                {
                    commandOutput.Settings.Merge(new Map(receipt), FfmpegMergeOptionType.NewWins);
                }
                else
                {
                    var isAudioResource =
                        stage.Command.Objects.Inputs.First(r => r.Resource.Map == receipt.Map).Resource is IAudio;
                    var mapSuffix = isAudioResource ? "a" : "v";
                    commandOutput.Settings.Merge(new Map(string.Format("{0}:{1}", indexOfResource, mapSuffix)),
                                                 FfmpegMergeOptionType.NewWins);
                }
            });

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                commandOutput.Resource.Name = fileName;
            }

            stage.Command.OutputManager.Add(commandOutput);

            outputObjects.Add(commandOutput);

            return outputObjects;
        }
        public static void ValidateTo(FfmpegCommand command)
        {
            if (command.Owner == null)
            {
                throw new ArgumentException("Command must contain an owner before sugar is allowed.", "command");
            }
        }
        
        public static List<CommandOutput> To<TOutputType>(this CommandStage stage)
            where TOutputType : class, IResource, new()
        {
            return stage.To<TOutputType>(SettingsCollection.ForOutput());
        }
        public static List<CommandOutput> To<TOutputType>(this CommandStage stage, SettingsCollection settings)
            where TOutputType : class, IResource, new()
        {
            return stage.To<TOutputType>(string.Empty, settings);
        }
        public static List<CommandOutput> To<TOutputType>(this CommandStage stage, string fileName, SettingsCollection settings)
            where TOutputType : class, IResource, new()
        {
            ValidateTo(stage.Command);

            var settingsCopy = settings.Copy();
            var outputObjects = new List<CommandOutput>();

            var commandOutput =
                CommandOutput.Create(Resource.CreateOutput<TOutputType>(), settingsCopy);

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                commandOutput.Resource.Name = fileName;
            }

            stage.Command.OutputManager.Add(commandOutput);

            outputObjects.Add(commandOutput);

            return outputObjects;
        }

        public static CommandStage BeforeRender(this CommandStage command, Action<CommandFactory, FfmpegCommand, bool> action)
        {
            command.Command.PreRenderAction = action;

            return command; 
        }
        public static CommandStage AfterRender(this CommandStage command, Action<CommandFactory, FfmpegCommand, bool> action)
        {
            command.Command.PostRenderAction = action;

            return command;
        }
    }
}