using System.Collections.Generic;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Resources.Interfaces;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    public abstract class BaseMovie : IFilter, IMetadataManipulation
    {
        [FilterParameter(Name = "filename")]
        [Validate(LogicalOperators.NotEquals, null)]
        public IContainer Resource { get; set; }

        [FilterParameter(Name = "f")]
        public string FormatName { get; set; }

        [FilterParameter(Name = "s")]
        public string Streams { get; set; }

        [FilterParameter(Name = "sp")]
        [Validate(LogicalOperators.GreaterThanOrEqual, 0)]
        public double? SeekPoint { get; set; }

        [FilterParameter(Name = "si")]
        [Validate(LogicalOperators.GreaterThanOrEqual, 0)]
        public int? StreamIndex { get; set; }

        [FilterParameter(Name = "loop")]
        [Validate(LogicalOperators.GreaterThanOrEqual, 1)]
        public int? Loop { get; set; }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            return MetadataInfoTreeContainer.Create(Resource);
        }
    }
}
