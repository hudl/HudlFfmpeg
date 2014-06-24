using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Overlay Filter that will overlay a video or image on another video or image.
    /// </summary>
    [AppliesToResource(Type = typeof(IVideo))]
    [AppliesToResource(Type = typeof(IImage))]
    public class Overlay : BaseFilter
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

        //TODO: legacy
        public override TimeSpan? LengthFromInputs(List<CommandResource> resources)
        {
            return Shortest
                ? resources.Min(r => r.Resource.Info.Duration)
                : resources.Max(r => r.Resource.Info.Duration);
        }
    }
}
