using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    [AppliesToResource(Type = typeof(IVideo))]
    [AppliesToResource(Type = typeof(IImage))]
    public class Overlay : IFilter
    {
        /// <summary>
        /// the video blend options, required value for blend command
        /// </summary>
        public enum EvalTypes 
        {
            frame,
            init
        }
        
        /// <summary>
        /// the blend all model to be used
        /// </summary>
        public enum FormatTypes 
        {
            yuv420,
            yuv444,
            rgb
        }

        public string X { get; set; }
        
        public string Y { get; set; } 
        
        public bool Shortest { get; set; } 
        
        public bool RepeatLast { get; set; } 
        
        public FormatTypes Format { get; set; } 
        
        public EvalTypes Eval { get; set; } 

        public string Type { get { return "overlay"; } }

        public int MaxInputs { get { return 1; } }

        public override string ToString() 
        {
            StringBuilder filter = new StringBuilder(100);
            if (!string.IsNullOrWhiteSpace(X))  
                filter.AppendFormat("{1}x={0}", 
                    X, 
                    (filter.Length > 0) ? ":" : string.Empty);
            if (!string.IsNullOrWhiteSpace(Y))  
                filter.AppendFormat("{1}y={0}", 
                    Y, 
                    (filter.Length > 0) ? ":" : string.Empty);
            if (Eval != EvalTypes.frame)  
                filter.AppendFormat("{1}eval={0}", 
                    Eval.ToString(), 
                    (filter.Length > 0) ? ":" : string.Empty);
            if (Format != FormatTypes.yuv420)  
                filter.AppendFormat("{1}format={0}", 
                    Format.ToString(), 
                    (filter.Length > 0) ? ":" : string.Empty);
            if (Shortest)  
                filter.AppendFormat("{1}shortest={0}", 
                    Convert.ToInt32(Shortest),
                    (filter.Length > 0) ? ":" : string.Empty);
            if (RepeatLast)  
                filter.AppendFormat("{1}repeatlast={0}", 
                    Convert.ToInt32(RepeatLast), 
                    (filter.Length > 0) ? ":" : string.Empty);

            return string.Concat(Type, "=", filter.ToString());
        }
    }
}
