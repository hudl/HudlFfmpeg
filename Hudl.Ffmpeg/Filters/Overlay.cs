using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Formatters;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Overlay Filter that will overlay a video or image on another video or image.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name = "overlay", MinInputs = 2, MaxInputs = 2)]
    public class Overlay : IFilter, IMetadataManipulation
    {
        public Overlay()
        {
            Format = OverlayVideoFormatType.Yuv420; 
            Eval = OverlayVideoEvalType.Frame;
        }
        public Overlay(int x, int y)
            : this()
        {
            X = x.ToString(CultureInfo.InvariantCulture);
            Y = y.ToString(CultureInfo.InvariantCulture);
        }

        [FilterParameter(Name = "x")]
        public string X { get; set; }

        [FilterParameter(Name = "y")]
        public string Y { get; set; }

        [FilterParameter(Name = "shortest", Default = false, Formatter = typeof(BoolToInt32Formatter))]
        public bool Shortest { get; set; }

        [FilterParameter(Name = "repeatlast", Default = false, Formatter = typeof(BoolToInt32Formatter))]
        public bool RepeatLast { get; set; }
       
        [FilterParameter(Name ="eval", Default = OverlayVideoEvalType.Frame, Formatter = typeof(EnumParameterFormatter))]
        public OverlayVideoEvalType? Eval { get; set; }

        [FilterParameter(Name ="eof_action", Default = OverlayEofActionType.Repeat, Formatter = typeof(EnumParameterFormatter))]
        public OverlayEofActionType? EofAction { get; set; }
        
        [FilterParameter(Name ="format", Default = OverlayVideoFormatType.Yuv420, Formatter = typeof(EnumParameterFormatter))]
        public OverlayVideoFormatType? Format { get; set; }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            if (Shortest)
            {
                return suppliedInfo.OrderBy(r => r.VideoStream.VideoMetadata.Duration).FirstOrDefault();
            }

            var mainMetadataInfo = suppliedInfo.ElementAt(0);
            var overlayMetadataInfo = suppliedInfo.ElementAt(1);

            return EofAction == OverlayEofActionType.EndAll 
                ? overlayMetadataInfo 
                : mainMetadataInfo;
        }
    }
}
