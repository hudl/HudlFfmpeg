using System.Collections.Generic;
using System.Linq;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Metadata.BaseTypes;

namespace Hudl.Ffmpeg.Metadata
{
    internal class MetadataInfoStreamCalculator
    {
        private MetadataInfoStreamCalculator(MetadataInfoTreeItem infoStream)
        {
            InfoStream = infoStream;
        }

        public MetadataInfoTreeItem InfoStream { get; private set; }

        public static MetadataInfoStreamCalculator Create(FfmpegCommand command, Filterchain filterchain)
        {
            var infoStreamItem = MetadataInfoTreeGroup.Create(command, filterchain);

            return new MetadataInfoStreamCalculator(infoStreamItem);
        }

        public static MetadataInfoStreamCalculator Create(FfmpegCommand command, CommandOutput commandOutput)
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

            var metadataInfoResult = metadataInfoTreeSource.Resource.Info.Copy();

            metadataManipulators.ForEach(mm => metadataInfoResult = mm.EditInfo(metadataInfoResult, new List<MetadataInfo> { metadataInfoTreeSource.ResultMetadataInfo }));

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
                var metadataManipulators = metadataInfoTreeGroup.Filterchain.Filters.List.OfType<IMetadataManipulation>().ToList();

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
