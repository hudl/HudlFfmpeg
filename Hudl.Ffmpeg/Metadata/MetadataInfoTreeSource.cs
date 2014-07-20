using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Metadata
{
    internal class MetadataInfoTreeSource : MetadataInfoTreeItem
    {
        private MetadataInfoTreeSource(IContainer resource, SettingsCollection settings)
        {
            Settings = settings;
            Resource = MetadataInfoTreeContainer.Create(resource);
        }

        public MetadataInfoTreeContainer Resource { get; private set; }

        public SettingsCollection Settings { get; set; }

        public static MetadataInfoTreeSource Create(CommandInput commandResource)
        {
            var treeSourceFromInput = new MetadataInfoTreeSource(commandResource.Resource, commandResource.Settings);

            return treeSourceFromInput;
        }
    }

}
