using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.MediaInfo;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;
using Hudl.Ffmpeg.Resources; 

namespace Hudl.Ffmpeg.Command
{
    public static class FfmpegSugar
    {
        #region CommandFactory Sugar
        public static Commandv2 CreateOutput(this CommandFactory factory)
        {
            var newCommand = Commandv2.Create(factory);
            factory.AddToOutput(newCommand);
            return newCommand;
        }
        public static Commandv2 CreateResource(this CommandFactory factory)
        {
            var newCommand = Commandv2.Create(factory);
            factory.AddToResources(newCommand);
            return newCommand;
        }
        #endregion

        #region Commandv2 Sugar
        public static void ValidateGiving(Commandv2 command, string fileName, SettingsCollection collection)
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
        public static Commandv2 GivingVideo(this Commandv2 command, string fileName)
        {
            return command.GivingVideo(fileName, SettingsCollection.ForOutput()); 
        }
        public static Commandv2 GivingVideo(this Commandv2 command, List<string> files)
        {
            return command.GivingVideo(files, SettingsCollection.ForOutput());
        }
        public static Commandv2 GivingVideo(this Commandv2 command, string fileName, SettingsCollection collection)
        {
            ValidateGiving(command, fileName, collection);

            var videoResource = Resource.VideoFrom(fileName);

            var commandOutput = CommandOutput.Create(videoResource);

            command.OutputManager.Add(commandOutput);

            return command;
        }
        public static Commandv2 GivingVideo(this Commandv2 command, List<string> files, SettingsCollection collection)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Files cannot be null or empty.", "files");
            }

            files.ForEach(fileName => command.GivingVideo(fileName, collection));

            return command;
        }

        public static Commandv2 GivingAudio(this Commandv2 command, string fileName)
        {
            return command.GivingAudio(fileName, SettingsCollection.ForOutput()); 
        }
        public static Commandv2 GivingAudio(this Commandv2 command, List<string> files)
        {
            return command.GivingAudio(files, SettingsCollection.ForOutput());
        }
        public static Commandv2 GivingAudio(this Commandv2 command, string fileName, SettingsCollection collection)
        {
            ValidateGiving(command, fileName, collection);

            var videoResource = Resource.AudioFrom(fileName);

            var commandOutput = CommandOutput.Create(videoResource);

            command.OutputManager.Add(commandOutput);

            return command;
        }
        public static Commandv2 GivingAudio(this Commandv2 command, List<string> files, SettingsCollection collection)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Files cannot be null or empty.", "files");
            }

            files.ForEach(fileName => command.GivingAudio(fileName, collection));

            return command;
        }

        public static Commandv2 GivingImage(this Commandv2 command, string fileName)
        {
            return command.GivingImage(fileName, SettingsCollection.ForOutput());
        }
        public static Commandv2 GivingImage(this Commandv2 command, List<string> files)
        {
            return command.GivingImage(files, SettingsCollection.ForOutput());
        }
        public static Commandv2 GivingImage(this Commandv2 command, string fileName, SettingsCollection collection)
        {
            ValidateGiving(command, fileName, collection);

            var videoResource = Resource.ImageFrom(fileName);

            var commandOutput = CommandOutput.Create(videoResource);

            command.OutputManager.Add(commandOutput);

            return command; 
        }
        public static Commandv2 GivingImage(this Commandv2 command, List<string> files, SettingsCollection collection)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Files cannot be null or empty.", "files");
            }

            files.ForEach(fileName => command.GivingImage(fileName, collection));

            return command;
        }

        public static void ValidateUsing(Commandv2 command, string fileName)
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
        public static Commandv2 UsingVideo(this Commandv2 command, string fileName)
        {
            ValidateUsing(command, fileName);

            var videoResource = Resource.VideoFrom(fileName)
                .LoadMetadata();

            var commandResource = CommandResourcev2.Create(videoResource);

            command.ResourceManager.Add(commandResource);

            return command; 
        }
        public static Commandv2 UsingVideo(this Commandv2 command, List<string> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Files cannot be null or empty.", "files");
            }

            files.ForEach(fileName => command.UsingVideo(fileName));

            return command; 
        }

        public static Commandv2 UsingAudio(this Commandv2 command, string fileName)
        {
            ValidateUsing(command, fileName);

            var videoResource = Resource.AudioFrom(fileName)
                .LoadMetadata();

            var commandResource = CommandResourcev2.Create(videoResource);

            command.ResourceManager.Add(commandResource);

            return command; 
        }
        public static Commandv2 UsingAudio(this Commandv2 command, List<string> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Files cannot be null or empty.", "files");
            }

            files.ForEach(fileName => command.UsingAudio(fileName));

            return command;
        }

        public static Commandv2 UsingImage(this Commandv2 command, string fileName)
        {
            ValidateUsing(command, fileName);

            var videoResource = Resource.ImageFrom(fileName)
                .LoadMetadata();

            var commandResource = CommandResourcev2.Create(videoResource);

            command.ResourceManager.Add(commandResource);

            return command; 
        }
        public static Commandv2 UsingImage(this Commandv2 command, List<string> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Files cannot be null or empty.", "files");
            }

            files.ForEach(fileName => command.UsingImage(fileName));

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
        #endregion 

        #region Resource Sugar
        public static IResource LoadMetadata(this IResource resource)
        {
            var mediaLoader = new MediaLoader(resource.FullName);
            resource.Length = mediaLoader.Duration;
            return resource; 
        }
        #endregion
    }
}
