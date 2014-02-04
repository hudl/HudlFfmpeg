using System;
using System.Collections.Generic;
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
       
        public string X { get; set; }
        
        public string Y { get; set; } 
        
        public bool Shortest { get; set; } 
        
        public bool RepeatLast { get; set; }

        public OverlayVideoEvalType Eval { get; set; }
        
        public OverlayVideoFormatType Format { get; set; }

        public override TimeSpan? LengthFromInputs(List<CommandResource> resources)
        {
            return Shortest
                ? resources.Min(r => r.Resource.Length) 
                : resources.Max(r => r.Resource.Length);
        }

        public override string ToString() 
        {
            var filter = new StringBuilder(100);
            if (!string.IsNullOrWhiteSpace(X))
            {
                filter.AppendFormat("{1}x={0}", 
                    X, 
                    (filter.Length > 0) ? ":" : "=");
            }
            if (!string.IsNullOrWhiteSpace(Y))
            {
                filter.AppendFormat("{1}y={0}", 
                    Y,
                    (filter.Length > 0) ? ":" : "=");
            }
            if (Eval != OverlayVideoEvalType.Frame)  
            {
                filter.AppendFormat("{1}eval={0}", 
                    Eval.ToString().ToLower(),
                    (filter.Length > 0) ? ":" : "=");
            }
            if (Format != OverlayVideoFormatType.Yuv420)  
            {
                filter.AppendFormat("{1}format={0}", 
                    Format.ToString().ToLower(),
                    (filter.Length > 0) ? ":" : "=");
            }
            if (Shortest)  
            {
                filter.AppendFormat("{1}shortest={0}", 
                    Convert.ToInt32(Shortest),
                    (filter.Length > 0) ? ":" : "=");
            }
            if (RepeatLast)  
            {
                filter.AppendFormat("{1}repeatlast={0}", 
                    Convert.ToInt32(RepeatLast),
                    (filter.Length > 0) ? ":" : "=");
            }

            return string.Concat(Type, filter.ToString());
        }
    }
}
