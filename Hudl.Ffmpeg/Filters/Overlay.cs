using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Metadata;
using Hudl.Ffmpeg.Metadata.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Overlay Filter that will overlay a video or image on another video or image.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    public class Overlay : BaseFilter, IMetadataManipulation
    {
        private const int FilterMaxInputs = 2;
        private const string FilterType = "overlay";

        public Overlay()
            : base(FilterType, FilterMaxInputs)
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
       
        public string X { get; set; }
        
        public string Y { get; set; }

        public bool Shortest { get; set; } 
        
        public bool RepeatLast { get; set; }

        public OverlayVideoEvalType Eval { get; set; }

        public OverlayEofActionType EofAction { get; set; }
        
        public OverlayVideoFormatType Format { get; set; }

        public override string ToString() 
        {
            var filterParameters = new StringBuilder(100);

            if (!string.IsNullOrWhiteSpace(X))
            {
                FilterUtility.ConcatenateParameter(filterParameters, "x", X);
            }
            if (!string.IsNullOrWhiteSpace(Y))
            {
                FilterUtility.ConcatenateParameter(filterParameters, "y", Y);
            }
            if (Eval != OverlayVideoEvalType.Frame)  
            {
                FilterUtility.ConcatenateParameter(filterParameters, "eval", Formats.EnumValue(Eval));
            }
            if (Format != OverlayVideoFormatType.Yuv420)  
            {
                FilterUtility.ConcatenateParameter(filterParameters, "format", Formats.EnumValue(Format));
            }
            if (EofAction != OverlayEofActionType.Repeat)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "eof_action", Formats.EnumValue(EofAction));
            }
            if (Shortest)
            {
                FilterUtility.ConcatenateParameter(filterParameters, "shortest", 1);
            }
            if (RepeatLast)  
            {
                FilterUtility.ConcatenateParameter(filterParameters, "repeatlast", 1);
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }

        public MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            if (Shortest)
            {
                return suppliedInfo.OrderBy(r => r.VideoStream.Duration).FirstOrDefault();
            }

            var mainMetadataInfo = suppliedInfo.ElementAt(0);
            var overlayMetadataInfo = suppliedInfo.ElementAt(1);

            return EofAction == OverlayEofActionType.EndAll 
                ? overlayMetadataInfo 
                : mainMetadataInfo;
        }
    }
}
