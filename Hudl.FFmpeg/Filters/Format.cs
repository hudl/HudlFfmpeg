using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.DataTypes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Formats the Video stream
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name = "format", MinInputs = 1, MaxInputs = 1)]
    public class Format :IFilter
    {
        public Format()
        {
        }
        [FilterParameter( Formatter = typeof(EnumParameterFormatter))]
        public PixelFormatType PixelFormat { get; set; }
    }
}
