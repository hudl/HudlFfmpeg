using System;
using System.Collections.Generic;
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

            return stage.Command.WithReceipts(outputReceipts); 
        }
        public static CommandStage FilterEach(this CommandStage stage, Filterchain filterchain)
        {
            var outputReceipts = stage.Command.FilterchainManager.AddToEach(filterchain, stage.Receipts.ToArray());

            return stage.Command.WithReceipts(outputReceipts); 
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
            ValidateMapTo(stage.Command);

            var settingsCopy = settings.Copy();
            var outputObjects = new List<CommandOutput>();

            stage.Receipts.ForEach(receipt =>
                {
                    var commandOutput = CommandOutput.Create(Resource.CreateOutput<TOutputType>(stage.Command.Owner.Configuration), settingsCopy);

                    commandOutput.Settings.Merge(new Map(receipt), FfmpegMergeOptionType.NewWins); 

                    stage.Command.OutputManager.Add(commandOutput);

                    outputObjects.Add(commandOutput);
                });

            return outputObjects;
        }
    }
}
