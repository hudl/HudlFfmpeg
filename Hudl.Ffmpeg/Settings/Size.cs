using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings.Attributes;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFmpeg.Settings
{
    /// <summary>
    /// sets the outpout container size.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Setting(Name = "s")]
    public class Size : ISetting, IMetadataManipulation
    {
        public Size(ScalePresetType preset)
        {
            var scalingPresets = Helpers.ScalingPresets;
            if (!scalingPresets.ContainsKey(preset))
            {
                throw new ArgumentException("The preset does not currently exist.", "preset");
            }

            Width = scalingPresets[preset].Width;
            Height = scalingPresets[preset].Height;
        }
        public Size(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentException("Dimensions Width must be greater than zero.");
            }
            if (height <= 0)
            {
                throw new ArgumentException("Dimensions Height must be greater than zero.");
            }

            Width = width; 
            Height = height; 
        }

        [SettingParameter(Formatter = typeof(SizeWidthFormatter))]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public int? Width { get; set; }

        [SettingParameter(Formatter = typeof(SizeHeightFormatter))]
        [Validate(LogicalOperators.GreaterThan, 0)]
        public int? Height { get; set; }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            infoToUpdate.VideoStream.VideoMetadata.Width = Width.GetValueOrDefault();
            infoToUpdate.VideoStream.VideoMetadata.Height = Height.GetValueOrDefault();
            infoToUpdate.VideoStream.VideoMetadata.CodedWidth = Width.GetValueOrDefault();
            infoToUpdate.VideoStream.VideoMetadata.CodedHeight = Height.GetValueOrDefault();

            return infoToUpdate;
        }
    }
}
