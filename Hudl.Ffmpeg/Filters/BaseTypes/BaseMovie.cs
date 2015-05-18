
using System;
using System.Collections.Generic;
using System.Text;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFprobe.Metadata.BaseTypes;

namespace Hudl.FFmpeg.Filters.BaseTypes
{
    public abstract class BaseMovie : IFilter, IMetadataManipulation
    {
        [FilterParameter(Name = "filename")]
        [FilterParameterValidator(LogicalOperators.NotEquals, null)]
        public IContainer Resource { get; set; }

        [FilterParameter(Name = "f")]
        public string FormatName { get; set; }

        [FilterParameter(Name = "s")]
        public string Streams { get; set; }

        [FilterParameter(Name = "sp")]
        [FilterParameterValidator(LogicalOperators.GreaterThanOrEqual, 0)]
        public double? SeekPoint { get; set; }

        [FilterParameter(Name = "si")]
        [FilterParameterValidator(LogicalOperators.GreaterThanOrEqual, 0)]
        public int? StreamIndex { get; set; }

        [FilterParameter(Name = "loop")]
        [FilterParameterValidator(LogicalOperators.GreaterThanOrEqual, 1)]
        public int? Loop { get; set; }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            return MetadataInfoTreeContainer.Create(Resource);
        }
    }
}
