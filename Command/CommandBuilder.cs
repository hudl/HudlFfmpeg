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

        public void WriteCommand(Command<IResource> command)
        {
            command.CommandList.ForEach(WriteCommand);

            WriteStart();

            command.ResourceList.ForEach(WriteResource);

            WriteFiltergraph(command.Filtergraph);

            WriteOutput(command.Output);

            WriteFinish();
        }

        private void WriteStart()
        {
            _builderBase.Append(FfmpegMethodName);
        }

        private void WriteFinish()
        {
            _builderBase.AppendLine();
        }

        private void WriteResource(CommandResource<IResource> resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            var settingsData = Validate.GetSettingCollectionData(resource.Settings); 

            WriteResourcePreSettings(resource, settingsData);

            var inputResource = new Input(resource.Resource);
            _builderBase.Append(inputResource);

            WriteResourcePostSettings(resource, settingsData);
        }
        
        private void WriteResourcePreSettings(CommandResource<IResource> resource, Dictionary<Type, SettingsApplicationData> settingsData)
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
                    if (settingInfoData.ResourceType != SettingsCollectionResourceTypes.Input) return;

                    _builderBase.Append(setting);
                });
        }
      
        private void WriteResourcePostSettings(CommandResource<IResource> resource, Dictionary<Type, SettingsApplicationData> settingsData)
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
                if (settingInfoData.ResourceType != SettingsCollectionResourceTypes.Input) return;

                _builderBase.Append(setting);
            });

        }

        private void WriteFiltergraph(Filtergraph filtergraph)
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
                        _builderBase.Append("; ");
                    }
                    else
                    {
                        shouldIncludeDelimitor = true; 
                    }

                    WriteFilterchain(filterchain);
                });
        }
        
        private void WriteFilterchain(Filterchain<IResource> filterchain)
        {
            if (filterchain == null)
            {
                throw new ArgumentNullException("filterchain");
            }

            var shouldIncludeDelimitor = false;
            filterchain.Filters.List.ForEach(filter =>
                {
                    if (shouldIncludeDelimitor)
                    {
                        _builderBase.Append(",");
                    }
                    else
                    {
                        shouldIncludeDelimitor = true;
                    }

                    WriteFilter(filter);
                });
        }

        private void WriteFilter(IFilter filter)
        {
            if (filter == null)
            { 
                throw new ArgumentNullException("filter");
            }

            _builderBase.Append(filter.ToString());
        }

        private void WriteOutput(CommandOutput<IResource> output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            var settingsData = Validate.GetSettingCollectionData(output.Settings);

            WriteOutputSettings(output);

            _builderBase.AppendFormat(" {0}", Helpers.EscapePath(output.Output()));
        }

        private void WriteOutputSettings(CommandOutput<IResource> output)
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
                if (settingInfoData.ResourceType != SettingsCollectionResourceTypes.Output) return;

                _builderBase.Append(setting);
            });
        }

        public override string ToString()
        {
            return _builderBase.ToString();
        }
    }
}
