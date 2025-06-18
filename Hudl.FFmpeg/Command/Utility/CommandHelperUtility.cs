using System;
using System.Linq;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Exceptions;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Command.Utility;

internal class CommandHelperUtility
{
    public static bool ReceiptBelongsToCommand(FFmpegCommand command, StreamIdentifier streamId)
    {
        return command.Owner.Id == streamId.FactoryId
               && command.Id == streamId.CommandId;
    }

    public static int IndexOfFilterchain(FFmpegCommand command, StreamIdentifier streamId)
    {
        var matchingFilterchain = FilterchainFromStreamIdentifier(command, streamId);
        if (matchingFilterchain == null)
        {
            return -1;
        }

        return command.Filtergraph.IndexOf(matchingFilterchain);
    }

    public static int IndexOfResource(FFmpegCommand command, StreamIdentifier streamId)
    {
        var matchingResource = CommandInputFromStreamIdentifier(command, streamId);
        if (matchingResource == null)
        {
            return -1;
        }

        return command.Inputs.IndexOf(matchingResource);
    }

    public static int IndexOfOutput(FFmpegCommand command, StreamIdentifier streamId)
    {
        var matchingOutput = CommandOutputFromStreamIdentifier(command, streamId);
        if (matchingOutput == null)
        {
            return -1;
        }

        return command.Outputs.IndexOf(matchingOutput);
    }

    public static IStream StreamFromStreamIdentifier(FFmpegCommand command, StreamIdentifier streamId)
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

    public static CommandInput CommandInputFromStreamIdentifier(FFmpegCommand command, StreamIdentifier streamId)
    {
        if (streamId == null)
        {
            throw new ArgumentNullException("streamId");
        }

        return command.Objects.Inputs.FirstOrDefault(i => i.GetStreamIdentifiers().Any(si => si.Map == streamId.Map));
    }

    public static CommandOutput CommandOutputFromStreamIdentifier(FFmpegCommand command, StreamIdentifier streamId)
    {
        if (streamId == null)
        {
            throw new ArgumentNullException("streamId");
        }

        return command.Objects.Outputs.FirstOrDefault(i => i.GetStreamIdentifiers().Any(si => si.Map == streamId.Map));
    }

    public static Filterchain FilterchainFromStreamIdentifier(FFmpegCommand command, StreamIdentifier streamId)
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
                commandOutput.Settings.Merge(new Map(streamId), FFmpegMergeOptionType.NewWins);
            }
            else
            {
                var resourceIndex = IndexOfResource(stage.Command, streamId);

                commandOutput.Settings.Merge(new Map(string.Format("{0}:{1}", resourceIndex, theStream.ResourceIndicator)),
                    FFmpegMergeOptionType.NewWins);
            }

            commandOutput.Resource.Streams.Add(theStream.Copy());
        });

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            commandOutput.Resource.Name = fileName;
        }

        return commandOutput; 
    }

    public static CommandOutput SetupCommandOutput<TOutputType>(FFmpegCommand command, SettingsCollection settings, string fileName)
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