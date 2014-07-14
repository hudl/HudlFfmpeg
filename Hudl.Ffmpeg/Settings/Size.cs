using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Metadata;
using Hudl.Ffmpeg.Metadata.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings.BaseTypes;

namespace Hudl.Ffmpeg.Settings
{
    /// <summary>
    /// sets the outpout container size.
    /// </summary>
    [AppliesToResource(Type = typeof(IVideo))]
    [SettingsApplication(PreDeclaration = true, ResourceType = SettingsCollectionResourceType.Output)]
    public class Size : BaseSetting, IMetadataManipulation
    {
        private const string SettingType = "-s";

        public Size()
            : base(SettingType)
        {
            Dimensions = new System.Drawing.Size(0, 0);
        }
        public Size(ScalePresetType preset)
            : this()
        {
            var scalingPresets = Helpers.ScalingPresets;
            if (!scalingPresets.ContainsKey(preset))
            {
                throw new ArgumentException("The preset does not currently exist.", "preset");
            }

            Dimensions = scalingPresets[preset];
        }
        public Size(int x, int y)
            : this()
        {
            if (x <= 0)
            {
                throw new ArgumentException("Dimensions X must be greater than zero.");
            }
            if (y <= 0)
            {
                throw new ArgumentException("Dimensions Y must be greater than zero.");
            }

            Dimensions = new System.Drawing.Size(x, y);
        }

        public System.Drawing.Size Dimensions { get; set; }

        public override void Validate()
        {
            if (Dimensions == null)
            {
                throw new InvalidOperationException("Dimensions size cannot be null.");
            }
            if (Dimensions.Width <= 0)
            {
                throw new InvalidOperationException("Dimensions width must be greater than zero.");
            }
            if (Dimensions.Height <= 0)
            {
                throw new InvalidOperationException("Dimensions height must be greater than zero.");
            }
        }

        public override string ToString()
        {
            return string.Concat(Type, " ", Dimensions.Width, "x", Dimensions.Height);
        }

        public MetadataInfo EditInfo(MetadataInfo infoToUpdate, List<MetadataInfo> suppliedInfo)
        {
            infoToUpdate.Dimensions = Dimensions;

            return infoToUpdate;
        }
    }
}
