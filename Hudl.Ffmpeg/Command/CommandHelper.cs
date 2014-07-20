using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    internal class CommandHelper
    {
        public static bool ReceiptBelongsToCommand(FfmpegCommand command, StreamIdentifier streamId)
        {
            return command.Owner.Id == streamId.FactoryId
                   && command.Id == streamId.CommandId;
        }

        public static int IndexOfFilterchain(FfmpegCommand command, StreamIdentifier streamId)
        {
            var matchingFilterchain = FilterchainFromStreamIdentifier(command, streamId);
            if (matchingFilterchain == null)
            {
                return -1;
            }

            return command.Filtergraph.IndexOf(matchingFilterchain);
        }

        public static int IndexOfResource(FfmpegCommand command, StreamIdentifier streamId)
        {
            var matchingResource = CommandInputFromStreamIdentifier(command, streamId);
            if (matchingResource == null)
            {
                return -1;
            }

            return command.Inputs.IndexOf(matchingResource);
        }

        public static int IndexOfOutput(FfmpegCommand command, StreamIdentifier streamId)
        {
            var matchingOutput = CommandOutputFromStreamIdentifier(command, streamId);
            if (matchingOutput == null)
            {
                return -1;
            }

            return command.Outputs.IndexOf(matchingOutput);
        }

        public static IStream StreamFromStreamIdentifier(FfmpegCommand command, StreamIdentifier streamId)
        {
            var commandInput = CommandInputFromStreamIdentifier(command, streamId);
            if (commandInput != null)
            {
                return commandInput.Resource.Streams.FirstOrDefault(si => si.Map == streamId.Map);
            }

            var commandOutput = CommandOutputFromStreamIdentifier(command, streamId);
            if (commandOutput != null)
            {
                return commandOutput.Resource.Streams.FirstOrDefault(si => si.Map == streamId.Map);
            }

            var filterchain = FilterchainFromStreamIdentifier(command, streamId);
            if (filterchain != null)
            {
                var filterchainOutput = filterchain.OutputList.First(si => si.Stream.Map == streamId.Map);

                return filterchainOutput.Stream;
            }

            throw new StreamNotFoundException();
        }

        public static CommandInput CommandInputFromStreamIdentifier(FfmpegCommand command, StreamIdentifier streamId)
        {
            if (streamId == null)
            {
                throw new ArgumentNullException("streamId");
            }

            return command.Objects.Inputs.FirstOrDefault(i => i.GetStreamIdentifiers().Any(si => si.Map == streamId.Map));
        }

        public static CommandOutput CommandOutputFromStreamIdentifier(FfmpegCommand command, StreamIdentifier streamId)
        {
            if (streamId == null)
            {
                throw new ArgumentNullException("streamId");
            }

            return command.Objects.Outputs.FirstOrDefault(i => i.GetStreamIdentifiers().Any(si => si.Map == streamId.Map));
        }

        public static Filterchain FilterchainFromStreamIdentifier(FfmpegCommand command, StreamIdentifier streamId)
        {
            if (streamId == null)
            {
                throw new ArgumentNullException("streamId");
            }

            return command.Objects.Filtergraph.FilterchainList.FirstOrDefault(f => f.GetStreamIdentifiers().Any(r => r.Equals(streamId)));
        }

        public static CommandOutput SetupCommandOutputMaps<TOutputType>(CommandStage stage, SettingsCollection settings, string fileName)
            where TOutputType : class, IContainer, new()
        {
            var settingsCopy = settings.Copy();
            var commandOutput = CommandOutput.Create(Resource.CreateOutput<TOutputType>(), settingsCopy);

            stage.StreamIdentifiers.ForEach(streamId =>
            {
                var theStream = StreamFromStreamIdentifier(stage.Command, streamId);
                var theResource = CommandInputFromStreamIdentifier(stage.Command, streamId);

                if (theResource == null)
                {
                    commandOutput.Settings.Merge(new Map(streamId), FfmpegMergeOptionType.NewWins);
                }
                else
                {
                    var resourceIndex = IndexOfResource(stage.Command, streamId);

                    commandOutput.Settings.Merge(new Map(string.Format("{0}:{1}", resourceIndex, theStream.ResourceIndicator)),
                        FfmpegMergeOptionType.NewWins);
                }

                commandOutput.Resource.Streams.Add(theStream.Copy());
            });

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                commandOutput.Resource.Name = fileName;
            }

            return commandOutput; 
        }

        public static CommandOutput SetupCommandOutput<TOutputType>(FfmpegCommand command, SettingsCollection settings, string fileName)
            where TOutputType : class, IContainer, new()
        {
            var settingsCopy = settings.Copy();
            var commandOutput = CommandOutput.Create(Resource.CreateOutput<TOutputType>(), settingsCopy);

            commandOutput.Resource.Streams.AddRange(command.Inputs.SelectMany(i => i.Resource.Streams.Select(s => s.Copy())));

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                commandOutput.Resource.Name = fileName;
            }

            return commandOutput;
        }
    }
}
