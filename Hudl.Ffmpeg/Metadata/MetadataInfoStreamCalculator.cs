using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Metadata.Interfaces;

namespace Hudl.FFmpeg.Metadata
{
    internal class MetadataInfoStreamCalculator
    {
        private MetadataInfoStreamCalculator(MetadataInfoTreeItem infoStream)
        {
            InfoStream = infoStream;
        }

        public MetadataInfoTreeItem InfoStream { get; private set; }

        public static MetadataInfoStreamCalculator Create(FFmpegCommand command, Filterchain filterchain)
        {
            var infoStreamItem = MetadataInfoTreeGroup.Create(command, filterchain);

            return new MetadataInfoStreamCalculator(infoStreamItem);
        }

        public static MetadataInfoStreamCalculator Create(FFmpegCommand command, CommandOutput commandOutput)
        {
            var infoStreamItem = MetadataInfoTreeGroup.Create(command, commandOutput);

            return new MetadataInfoStreamCalculator(infoStreamItem);
        }

        public static MetadataInfoStreamCalculator Create(CommandInput commandResource)
        {
            var infoStreamItem = MetadataInfoTreeSource.Create(commandResource);

            return new MetadataInfoStreamCalculator(infoStreamItem);
        }

        public void Calculate()
        {
            Calculate((dynamic)InfoStream);
        }

        private static void Calculate(MetadataInfoTreeSource metadataInfoTreeSource)
        {
            var metadataManipulators = metadataInfoTreeSource.Settings.SettingsList.OfType<IMetadataManipulation>().ToList();

            var metadataInfoResult = metadataInfoTreeSource.Resource.Copy();

            metadataManipulators.ForEach(mm => metadataInfoResult = mm.EditInfo(metadataInfoResult, new List<MetadataInfoTreeContainer> { metadataInfoTreeSource.ResultMetadataInfo }));

            metadataInfoTreeSource.ResultMetadataInfo = metadataInfoResult;
        }

        private static void Calculate(MetadataInfoTreeGroup metadataInfoTreeGroup)
        {
            //first ensure that all dependencies have calculated metadata 
            if (metadataInfoTreeGroup.DependecyTree.Any(mti => mti.ResultMetadataInfo == null))
            {
                metadataInfoTreeGroup.DependecyTree.ForEach(mti => Calculate((dynamic)mti));
            }

            //second we need to form a list of the supplied metadata info 
            var suppliedMetadataInfo = metadataInfoTreeGroup.DependecyTree.Select(mti => mti.ResultMetadataInfo).ToList();

            var metadataInfoResult = suppliedMetadataInfo.First().Copy();

            //now we must execute the metadata manipulators for filters 
            if (metadataInfoTreeGroup.Filterchain != null)
            {
                var metadataManipulators = metadataInfoTreeGroup.Filterchain.Filters.OfType<IMetadataManipulation>().ToList();

                metadataManipulators.ForEach(mm => metadataInfoResult = mm.EditInfo(metadataInfoResult, suppliedMetadataInfo));
            }

            //now we must execute the metadata manipulators for settings
            if (metadataInfoTreeGroup.Settings != null)
            {
                var metadataManipulators = metadataInfoTreeGroup.Settings.SettingsList.OfType<IMetadataManipulation>().ToList();

                metadataManipulators.ForEach(mm => metadataInfoResult = mm.EditInfo(metadataInfoResult, suppliedMetadataInfo));
            }

            //finally set the calculated result.
            metadataInfoTreeGroup.ResultMetadataInfo = metadataInfoResult;
        }
    }
}
