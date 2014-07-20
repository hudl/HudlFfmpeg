using System.Collections.Generic;
using System.Linq;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Metadata
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

        public static MetadataInfoTreeGroup Create(FfmpegCommand command, Filterchain filterchain)
        {
            var metadataInfoTreeGroup = new MetadataInfoTreeGroup(filterchain);

            metadataInfoTreeGroup.Fill(command, filterchain);

            return metadataInfoTreeGroup;
        }

        public static MetadataInfoTreeGroup Create(FfmpegCommand command, CommandOutput commandOutput)
        {
            var metadataInfoTreeGroup = new MetadataInfoTreeGroup(commandOutput.Settings);

            metadataInfoTreeGroup.Fill(command, commandOutput);

            return metadataInfoTreeGroup;
        }

        private void Fill(FfmpegCommand command, Filterchain filterchain)
        {
            filterchain.ReceiptList.ForEach(streamId =>
            {
                var resourceIndexOf = CommandHelper.IndexOfResource(command, streamId);
                if (resourceIndexOf > -1)
                {
                    DependecyTree.Add(MetadataInfoTreeSource.Create(command.Inputs[resourceIndexOf]));
                }

                var filterchainIndexOf = CommandHelper.IndexOfFilterchain(command, streamId);
                if (filterchainIndexOf > -1)
                {
                    DependecyTree.Add(Create(command, command.Filtergraph[filterchainIndexOf]));
                }
            });
        }

        private void Fill(FfmpegCommand command, CommandOutput commandOutput)
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

        private void Fill(FfmpegCommand command, List<StreamIdentifier> streamIdList)
        {
            streamIdList.ForEach(streamId =>
            {
                var resourceIndexOf = CommandHelper.IndexOfResource(command, streamId);
                if (resourceIndexOf > -1)
                {
                    DependecyTree.Add(MetadataInfoTreeSource.Create(command.Inputs[resourceIndexOf]));

                    return;
                }

                var filterchainIndexOf = CommandHelper.IndexOfFilterchain(command, streamId);
                if (filterchainIndexOf > -1)
                {
                    DependecyTree.Add(Create(command, command.Filtergraph[filterchainIndexOf]));
                }
            });
        }
    }

}
