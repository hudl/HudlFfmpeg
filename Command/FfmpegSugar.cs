using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.MediaInfo;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;
using Hudl.Ffmpeg.Resources; 

namespace Hudl.Ffmpeg.Command
{
    public static class FfmpegSugar
    {
        #region CommandFactory Sugar
        public static Commandv2 AsOutput(this CommandFactory factory)
        {
            var newCommand = Commandv2.Create(factory);
            factory.AddToOutput(newCommand);
            return newCommand;
        }
        public static Commandv2 AsResource(this CommandFactory factory)
        {
            var newCommand = Commandv2.Create(factory);
            factory.AddToResources(newCommand);
            return newCommand;
        }
        #endregion

        #region Commandv2 Sugar
        public static void ValidateOutput(Commandv2 command, string fileName, SettingsCollection collection)
        {
            if (command.Owner == null)
            {
                throw new ArgumentException("Command must contain an owner before sugar is allowed.", "command");
            }

            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            if (collection.Type != SettingsCollectionResourceType.Output)
            {
                throw new ArgumentException("Command output settings collection must be a type of Output.", "collection");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Command fileName cannot be null or empty.", "fileName");
            }

            if (command.Owner.CommandList.All(c => c.Id != command.Id))
            {
                throw new ArgumentException("Command must be added via CreateOutput or CreateResource first.", "command");
            }
        }
        public static Commandv2 WithOutput(this Commandv2 command, string fileName)
        {
            return command.WithOutput(fileName, SettingsCollection.ForOutput());
        }
        public static Commandv2 WithOutput(this Commandv2 command, List<string> files)
        {
            return command.WithOutput(files, SettingsCollection.ForOutput());
        }
        public static Commandv2 WithOutput(this Commandv2 command, string fileName, SettingsCollection collection)
        {
            ValidateOutput(command, fileName, collection);

            var resource = Resource.From(fileName);

            var commandOutput = CommandOutput.Create(resource);

            command.OutputManager.Add(commandOutput);

            return command;
        }
        public static Commandv2 WithOutput(this Commandv2 command, List<string> files, SettingsCollection collection)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Files cannot be null or empty.", "files");
            }

            files.ForEach(fileName => command.WithOutput(fileName, collection));

            return command;
        }

        public static void ValidateInput(Commandv2 command, string fileName)
        {
            if (command.Owner == null)
            {
                throw new ArgumentException("Command must contain an owner before sugar is allowed.", "command");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Command fileName cannot be null or empty.", "fileName");
            }

            if (command.Owner.CommandList.All(c => c.Id != command.Id))
            {
                throw new ArgumentException("Command must be added via CreateOutput or CreateResource first.", "command");
            }
        }
        public static Commandv2 WithInput(this Commandv2 command, string fileName)
        {
            ValidateInput(command, fileName);

            var resource = Resource.From(fileName)
                .LoadMetadata();

            var commandResource = CommandResourcev2.Create(resource);

            command.ResourceManager.Add(commandResource);

            return command;
        }
        public static Commandv2 WithInput(this Commandv2 command, List<string> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Files cannot be null or empty.", "files");
            }

            files.ForEach(fileName => command.WithInput(fileName));

            return command;
        }

        public static CommandReceipt ResourceReceiptAt(this Commandv2 command, int index)
        {
            if (index < 0 || index >= command.Resources.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return command.Resources[index].GetReceipt(); 
        }

        public static void ValidateRecipts(Commandv2 command, List<CommandReceipt> receipts)
        {
            if (command.Owner == null)
            {
                throw new ArgumentException("Command must contain an owner before sugar is allowed.", "command");
            }

            if (receipts == null)
            {
                throw new ArgumentNullException("receipts");
            }
        }
        public static CommandStage WithReceipts(this Commandv2 command, params CommandReceipt[] receipts)
        {
            var receiptList = new List<CommandReceipt>(receipts);
            return command.WithReceipts(receiptList);
        }
        public static CommandStage WithReceipts(this Commandv2 command, List<CommandReceipt> receipts)
        {
            ValidateRecipts(command, receipts);

            return new CommandStage(command)
            {
                Receipts = receipts
            };
        }
        public static CommandStage WithReceiptsFrom(this Commandv2 command, CommandResourcev2 resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            return command.WithReceipts(resource.GetReceipt());
        }
        public static CommandStage WithReceiptsFrom(this Commandv2 command, Filterchainv2 filterchain)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            return command.WithReceipts(filterchain.GetReceipts());
        }
        #endregion 

        #region Resource Sugar
        public static IResource LoadMetadata(this IResource resource)
        {
            var mediaLoader = new MediaLoader(resource.FullName);

            resource.Info = MetadataInfo.Create(mediaLoader); 
            
            return resource; 
        }
        #endregion

        #region CommandStage 
        public static void ValidateFilter(Commandv2 command, Filterchainv2 filterchain)
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
        public static CommandStage Filter(this CommandStage stage, Filterchainv2 filterchain)
        {
            var outputReceipts = stage.Command.FilterchainManager.Add(filterchain, stage.Receipts.ToArray());

            return stage.Command.WithReceipts(outputReceipts); 
        }
        public static CommandStage FilterEach(this CommandStage stage, Filterchainv2 filterchain)
        {
            var outputReceipts = stage.Command.FilterchainManager.AddToEach(filterchain, stage.Receipts.ToArray());

            return stage.Command.WithReceipts(outputReceipts); 
        }

        public static void ValidateMapTo(Commandv2 command)
        {
            if (command.Owner == null)
            {
                throw new ArgumentException("Command must contain an owner before sugar is allowed.", "command");
            }
        }
        public static Commandv2 MapTo<TOutputType>(this CommandStage stage)
            where TOutputType : class, IResource, new()
        {
            return stage.MapTo<TOutputType>(SettingsCollection.ForOutput());
        }
        public static Commandv2 MapTo<TOutputType>(this CommandStage stage, SettingsCollection settings)
            where TOutputType : class, IResource, new()
        {
            ValidateMapTo(stage.Command);

            stage.Receipts.ForEach(receipt =>
                {
                    var commandOutput = CommandOutput.Create(new TOutputType(), settings);

                    commandOutput.Settings.Merge(new Map(receipt), FfmpegMergeOptionType.NewWins); 

                    stage.Command.OutputManager.Add(commandOutput);
                });

            return stage.Command;
        }
        #endregion
    }

    public class CommandStage
    {
        internal CommandStage(Commandv2 stageCommand)
        {
            Command = stageCommand; 
            Receipts = new List<CommandReceipt>();
        } 

        public Commandv2 Command { get; set; }

        public List<CommandReceipt> Receipts { get; set; }
    }

}
