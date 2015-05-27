using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Command.Models;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Command
{
    internal class FFmpegCommandBuilder: FFcommandBuilderBase, ICommandBuilder
    {
        public void WriteCommand(ICommand command)
        {
            WriteCommand((FFmpegCommand)command);
        }

        public void WriteCommand(FFmpegCommand command)
        {
            command.Objects.Inputs.ForEach(WriteResource);

            WriteFiltergraph(command, command.Objects.Filtergraph);

            command.Objects.Outputs.ForEach(WriteOutput);

            WriteFinish();
        }
        private void WriteResource(CommandInput resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            var settingsData = Validate.GetSettingCollectionData(resource.Settings);

            WriteResourcePreSettings(resource, settingsData);

            var inputResource = new Input(resource.Resource);
            BuilderBase.Append(" ");
            BuilderBase.Append(inputResource);

            WriteResourcePostSettings(resource, settingsData);
        }
        private void WriteResourcePreSettings(CommandInput resource, Dictionary<Type, SettingsApplicationData> settingsData)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            resource.Settings.SettingsList.ForEach(setting =>
            {
                var settingInfoData = settingsData[setting.GetType()];
                if (settingInfoData == null) return;
                if (!settingInfoData.PreDeclaration) return;
                if (settingInfoData.ResourceType != SettingsCollectionResourceType.Input) return;

                BuilderBase.Append(" ");
                BuilderBase.Append(setting.GetAndValidateString());
            });
        }
        private void WriteResourcePostSettings(CommandInput resource, Dictionary<Type, SettingsApplicationData> settingsData)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            resource.Settings.SettingsList.ForEach(setting =>
            {
                var settingInfoData = settingsData[setting.GetType()];
                if (settingInfoData == null) return;
                if (settingInfoData.PreDeclaration) return;
                if (settingInfoData.ResourceType != SettingsCollectionResourceType.Input) return;

                BuilderBase.Append(" ");
                BuilderBase.Append(setting.GetAndValidateString());
            });

        }
        private void WriteFiltergraph(FFmpegCommand command, Filtergraph filtergraph)
        {
            if (filtergraph == null)
            {
                throw new ArgumentNullException("filtergraph");
            }

            var shouldIncludeDelimitor = false;
            filtergraph.FilterchainList.ForEach(filterchain =>
            {
                if (shouldIncludeDelimitor)
                {
                    BuilderBase.Append(";");
                }
                else
                {
                    BuilderBase.Append(" -filter_complex \"");
                    shouldIncludeDelimitor = true;
                }

                WriteFilterchain(command, filterchain);
            });

            if (shouldIncludeDelimitor)
            {
                BuilderBase.Append("\"");
            }
        }
        private void WriteFilterchain(FFmpegCommand command, Filterchain filterchain)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            WriteFilterchainIn(command, filterchain);

            var shouldIncludeDelimitor = false;
            filterchain.Filters.List.ForEach(filter =>
            {
                if (shouldIncludeDelimitor)
                {
                    BuilderBase.Append(",");
                }
                else
                {
                    BuilderBase.Append(" ");
                    shouldIncludeDelimitor = true;
                }

                //TODO: fix
                //filter.Setup(command, filterchain);
                WriteFilter(filter);
            });

            WriteFilterchainOut(filterchain);
        }
        private void WriteFilterchainIn(FFmpegCommand command, Filterchain filterchain)
        {
            filterchain.ReceiptList.ForEach(streamId =>
            {
                BuilderBase.Append(" ");
                var indexOfResource = command.Objects.Inputs.FindIndex(inputs => inputs.GetStreamIdentifiers().Any(s => s.Map == streamId.Map));
                if (indexOfResource >= 0)
                {
                    var commandResource = command.Objects.Inputs[indexOfResource];
                    var commandStream = commandResource.Resource.Streams.First(s => s.Map == streamId.Map);
                    BuilderBase.Append(Formats.Map(commandStream, indexOfResource));
                }
                else
                {
                    BuilderBase.Append(Formats.Map(streamId.Map));
                }
            });
        }
        private void WriteFilterchainOut(Filterchain filterchain)
        {
            var filterchainOutputs = filterchain.GetStreamIdentifiers(); 
            filterchainOutputs.ForEach(streamId =>
                {
                    BuilderBase.Append(" ");
                    BuilderBase.Append(Formats.Map(streamId.Map));
                });
        }
        private void WriteOutput(CommandOutput output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            WriteOutputSettings(output);

            BuilderBase.AppendFormat(" {0}", Helpers.EscapePath(output.Resource));
        }
        private void WriteOutputSettings(CommandOutput output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            var settingsData = Validate.GetSettingCollectionData(output.Settings);
            output.Settings.SettingsList.ForEach(setting =>
            {
                var settingInfoData = settingsData[setting.GetType()];
                if (settingInfoData == null) return;
                if (!settingInfoData.PreDeclaration) return;
                if (settingInfoData.ResourceType != SettingsCollectionResourceType.Output) return;

                BuilderBase.Append(" ");
                BuilderBase.Append(setting.GetAndValidateString());
            });
        }

        private void WriteFinish()
        {
            BuilderBase.AppendLine();
        }
        private void WriteFilter(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            //TODO: fix
            //BuilderBase.Append(filter.GetAndValidateString());
        }
    }
}
