using System;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
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
            Format = OverlayVideoFormatTypes.Yuv420; 
            Eval = OverlayVideoEvalTypes.Frame;
        }
       
        public string X { get; set; }
        
        public string Y { get; set; } 
        
        public bool Shortest { get; set; } 
        
        public bool RepeatLast { get; set; }

        public OverlayVideoEvalTypes Eval { get; set; }
        
        public OverlayVideoFormatTypes Format { get; set; } 

        public override string ToString() 
        {
            var filter = new StringBuilder(100);
            if (!string.IsNullOrWhiteSpace(X))
            {
                filter.AppendFormat("{1}x={0}", 
                    X, 
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (!string.IsNullOrWhiteSpace(Y))
            {
                filter.AppendFormat("{1}y={0}", 
                    Y, 
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Eval != EvalTypes.Frame)  
            {
                filter.AppendFormat("{1}eval={0}", 
                    Eval.ToString().ToLower(), 
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Format != FormatTypes.Yuv420)  
            {
                filter.AppendFormat("{1}format={0}", 
                    Format.ToString().ToLower(), 
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (Shortest)  
            {
                filter.AppendFormat("{1}shortest={0}", 
                    Convert.ToInt32(Shortest),
                    (filter.Length > 0) ? ":" : string.Empty);
            }
            if (RepeatLast)  
            {
                filter.AppendFormat("{1}repeatlast={0}", 
                    Convert.ToInt32(RepeatLast), 
                    (filter.Length > 0) ? ":" : string.Empty);
            }

            return string.Concat(Type, "=", filter.ToString());
        }
    }
}
