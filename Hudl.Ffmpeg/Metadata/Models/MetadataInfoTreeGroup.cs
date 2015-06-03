using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Command.Utility;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;

namespace Hudl.FFmpeg.Metadata
{
    internal class MetadataInfoTreeGroup : MetadataInfoTreeItem
    {
        private MetadataInfoTreeGroup(Filterchain filterchain)
        {
            Filterchain = filterchain;
            DependecyTree = new List<MetadataInfoTreeItem>();
        }

        private MetadataInfoTreeGroup(SettingsCollection settings)
        {
            Settings = settings;
            DependecyTree = new List<MetadataInfoTreeItem>();
        }

        public List<MetadataInfoTreeItem> DependecyTree { get; private set; }

        public Filterchain Filterchain { get; private set; }

        public SettingsCollection Settings { get; set; }

        public static MetadataInfoTreeGroup Create(FFmpegCommand command, Filterchain filterchain)
        {
            var metadataInfoTreeGroup = new MetadataInfoTreeGroup(filterchain);

            metadataInfoTreeGroup.Fill(command, filterchain);

            return metadataInfoTreeGroup;
        }

        public static MetadataInfoTreeGroup Create(FFmpegCommand command, CommandOutput commandOutput)
        {
            var metadataInfoTreeGroup = new MetadataInfoTreeGroup(commandOutput.Settings);

            metadataInfoTreeGroup.Fill(command, commandOutput);

            return metadataInfoTreeGroup;
        }

        private void Fill(FFmpegCommand command, Filterchain filterchain)
        {
            filterchain.ReceiptList.ForEach(streamId =>
            {
                var resourceIndexOf = CommandHelperUtility.IndexOfResource(command, streamId);
                if (resourceIndexOf > -1)
                {
                    DependecyTree.Add(MetadataInfoTreeSource.Create(command.Inputs[resourceIndexOf]));
                }

                var filterchainIndexOf = CommandHelperUtility.IndexOfFilterchain(command, streamId);
                if (filterchainIndexOf > -1)
                {
                    DependecyTree.Add(Create(command, command.Filtergraph[filterchainIndexOf]));
                }
            });
        }

        private void Fill(FFmpegCommand command, CommandOutput commandOutput)
        {
            //find the command output map setting, if the command output has map settings 
            //then they make up its dependecy tree. 
            var allSettingMaps = commandOutput.Settings.OfType<Map>();
            if (allSettingMaps.Any())
            {
                var streamIdListFromMaps = allSettingMaps.Select(map => StreamIdentifier.Create(command.Owner.Id, command.Id, map.Stream)).ToList();

                Fill(command, streamIdListFromMaps);

                return;
            }

            //if the command output does not contain map settings then the dependency tree
            //is made up of all the input streams. 
            command.Objects.Inputs.ForEach(commandResource => DependecyTree.Add(MetadataInfoTreeSource.Create(commandResource)));
        }

        private void Fill(FFmpegCommand command, List<StreamIdentifier> streamIdList)
        {
            streamIdList.ForEach(streamId =>
            {
                var resourceIndexOf = CommandHelperUtility.IndexOfResource(command, streamId);
                if (resourceIndexOf > -1)
                {
                    DependecyTree.Add(MetadataInfoTreeSource.Create(command.Inputs[resourceIndexOf]));

                    return;
                }

                var filterchainIndexOf = CommandHelperUtility.IndexOfFilterchain(command, streamId);
                if (filterchainIndexOf > -1)
                {
                    DependecyTree.Add(Create(command, command.Filtergraph[filterchainIndexOf]));
                }
            });
        }
    }

}
