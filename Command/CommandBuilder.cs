using System;
using System.Collections.Generic;
using System.Text;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    internal class CommandBuilder
    {
        private const string FfmpegMethodName = "ffmpeg";
        private readonly StringBuilder _builderBase;

        public CommandBuilder()
        {
            _builderBase = new StringBuilder(100);            
        }

        public void WriteCommand(Commandv2 command)
        {
            command.Objects.Inputs.ForEach(WriteResource);

            WriteFiltergraph(command, command.Filtergraph);

            command.Objects.Outputs.ForEach(WriteOutput);

            WriteFinish();
        }
        private void WriteResource(CommandResourcev2 resource)
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
        private void WriteResourcePreSettings(CommandResourcev2 resource, Dictionary<Type, SettingsApplicationData> settingsData)
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
                _builderBase.Append(setting);
            });
        }
        private void WriteResourcePostSettings(CommandResourcev2 resource, Dictionary<Type, SettingsApplicationData> settingsData)
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
                _builderBase.Append(setting);
            });

        }
        private void WriteFiltergraph(Commandv2 command, Filtergraphv2 filtergraph)
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
        private void WriteFilterchain(Commandv2 command, Filterchainv2 filterchain)
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

            WriteFilterchainOut(command, filterchain);
        }
        private void WriteFilterchainIn(Commandv2 command, Filterchainv2 filterchain)
        {
            filterchain.ReceiptList.ForEach(receipt =>
            {
                _builderBase.Append(" ");
                var indexOfResource = command.Objects.Inputs.FindIndex(r => r.Resource.Map == receipt.Map);
                if (indexOfResource >= 0)
                {
                    var commandResource = command.Objects.Inputs[indexOfResource];
                    _builderBase.Append(Formats.Map(commandResource.Resource, indexOfResource));
                }
                else
                {
                    _builderBase.Append(Formats.Map(receipt.Map));
                }
            });
        }
        private void WriteFilterchainOut(Commandv2 command, Filterchainv2 filterchain)
        {
            var filterchainOutputs = filterchain.GetReceipts(); 
            filterchainOutputs.ForEach(receipt =>
                {
                    _builderBase.Append(" ");
                    _builderBase.Append(Formats.Map(receipt.Map));
                });
        }
        private void WriteOutput(CommandOutput output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            var settingsData = Validate.GetSettingCollectionData(output.Settings);

            WriteOutputSettings(output);

            _builderBase.AppendFormat(" {0}", Helpers.EscapePath(output.Output()));
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
                _builderBase.Append(setting);
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

            _builderBase.Append(filter.ToString());
        }

        public override string ToString()
        {
            return _builderBase.ToString();
        }
    }
}
