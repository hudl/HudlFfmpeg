using System;
using System.Linq;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Command.Models;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Filters.Serialization;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.Serialization;
using Hudl.FFmpeg.Settings.Utility;

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

            WriteResourcePreSettings(resource);

            var inputResource = new Input(resource.Resource);
            BuilderBase.Append(SettingSerializer.Serialize(inputResource));

            WriteResourcePostSettings(resource);
        }

        private void WriteResourcePreSettings(CommandInput resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            resource.Settings.SettingsList.ForEach(setting =>
            {
                if (!setting.IsPreSetting()) return; 

                BuilderBase.Append(SettingSerializer.Serialize(setting));
            });
        }
        private void WriteResourcePostSettings(CommandInput resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            resource.Settings.SettingsList.ForEach(setting =>
            {
                if (!setting.IsPostSetting()) return;

                BuilderBase.Append(SettingSerializer.Serialize(setting));
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

            output.Settings.SettingsList.ForEach(setting => BuilderBase.Append(SettingSerializer.Serialize(setting)));
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

            //TODO: fix, should need to check bindings
            BuilderBase.Append(FilterSerializer.Serialize(filter));
        }
    }
}
