using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Metadata.Ffprobe.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    internal class CommandBuilder
    {
        private readonly StringBuilder _builderBase;

        public CommandBuilder()
        {
            _builderBase = new StringBuilder(100);            
        }

        //ffprobe
        public void WriteCommand(FfprobeCommand command)
        {
            var inputResource = new Input(command.Resource);
            _builderBase.Append(" ");
            _builderBase.Append(inputResource);

            command.Serializers.ForEach(WriteSerializerSpecifier);
        }
        public void WriteSerializerSpecifier(IFfprobeSerializer serializer)
        {
            _builderBase.Append(" ");
            _builderBase.Append(serializer.Setting);   
        }

        //ffmpeg
        public void WriteCommand(FfmpegCommand command)
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
            _builderBase.Append(" ");
            _builderBase.Append(inputResource);

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

                _builderBase.Append(" ");
                _builderBase.Append(setting.GetAndValidateString());
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

                _builderBase.Append(" ");
                _builderBase.Append(setting.GetAndValidateString());
            });

        }
        private void WriteFiltergraph(FfmpegCommand command, Filtergraph filtergraph)
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
                    _builderBase.Append(";");
                }
                else
                {
                    _builderBase.Append(" -filter_complex \"");
                    shouldIncludeDelimitor = true;
                }

                WriteFilterchain(command, filterchain);
            });

            if (shouldIncludeDelimitor)
            {
                _builderBase.Append("\"");
            }
        }
        private void WriteFilterchain(FfmpegCommand command, Filterchain filterchain)
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
                    _builderBase.Append(",");
                }
                else
                {
                    _builderBase.Append(" ");
                    shouldIncludeDelimitor = true;
                }

                filter.Setup(command, filterchain);
                WriteFilter(filter);
            });

            WriteFilterchainOut(filterchain);
        }
        private void WriteFilterchainIn(FfmpegCommand command, Filterchain filterchain)
        {
            filterchain.ReceiptList.ForEach(streamId =>
            {
                _builderBase.Append(" ");
                var indexOfResource = command.Objects.Inputs.FindIndex(inputs => inputs.GetStreamIdentifiers().Any(s => s.Map == streamId.Map));
                if (indexOfResource >= 0)
                {
                    var commandResource = command.Objects.Inputs[indexOfResource];
                    var commandStream = commandResource.Resource.Streams.First(s => s.Map == streamId.Map);
                    _builderBase.Append(Formats.Map(commandStream, indexOfResource));
                }
                else
                {
                    _builderBase.Append(Formats.Map(streamId.Map));
                }
            });
        }
        private void WriteFilterchainOut(Filterchain filterchain)
        {
            var filterchainOutputs = filterchain.GetStreamIdentifiers(); 
            filterchainOutputs.ForEach(streamId =>
                {
                    _builderBase.Append(" ");
                    _builderBase.Append(Formats.Map(streamId.Map));
                });
        }
        private void WriteOutput(CommandOutput output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            WriteOutputSettings(output);

            _builderBase.AppendFormat(" {0}", Helpers.EscapePath(output.Resource));
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

                _builderBase.Append(" ");
                _builderBase.Append(setting.GetAndValidateString());
            });
        }

        //common 
        private void WriteFinish()
        {
            _builderBase.AppendLine();
        }
        private void WriteFilter(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            _builderBase.Append(filter.GetAndValidateString());
        }

        public override string ToString()
        {
            return _builderBase.ToString();
        }
    }
}
