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
    [AppliesToResource(Type=typeof(IVideo))]
    public class Blend : IFilter
    {
        public Blend() 
        {
            Mode = _mode;
            Option = _option;
        }
        public Blend(string expression) : this()
        {
            Expression = expression;
        }

        /// <summary>
        /// the video blend options, required value for blend command
        /// </summary>
        public enum BlendOptions 
        {
            c0_mode,
            c1_mode,
            c2_mode,
            c3_mode,
            all_mode,
            c0_opacity,
            c1_opacity,
            c2_opacity,
            c3_opacity,
            all_opacity,
            c0_expr,
            c1_expr,
            c2_expr,
            c3_expr,
            all_expr
        }
        
        /// <summary>
        /// the blend all model to be used
        /// </summary>
        public enum BlendMode 
        {
            addition,
            and,
            average,
            burn,
            darken,
            difference,
            divide,
            dodge,
            exclusion,
            hardlight,
            lighten,
            multiply,
            negation,
            normal,
            or,
            overlay,
            phoenix,
            pinlight,
            reflect,
            screen,
            softlight,
            subtract,
            vividlight,
            xor
        }

        public string Type { get { return "blend"; } }

        public int MaxInputs { get { return 2; } }

        public BlendOptions Option { get; set; }
        private BlendOptions _option = BlendOptions.all_expr;

        public BlendMode Mode { get; set; }
        private BlendMode _mode = BlendMode.and; 

        public string Expression { get; set; }

        public override string ToString() 
        {
            if (Option == BlendOptions.all_expr && string.IsNullOrWhiteSpace(Expression)) 
                throw new ArgumentException("Expression cannot be empty with Blend Option 'all_expr'", "Expression");

            StringBuilder filter = new StringBuilder(100);
            filter.AppendFormat("{0}", Option.ToString());
            switch (Option) 
            {
                case BlendOptions.all_expr:
                    filter.AppendFormat("='{0}'", Expression);
                    break;
                default: 
                    filter.AppendFormat("={0}", Mode.ToString());
                    break;
            }

            return string.Concat(Type, "=", filter.ToString());
        }
    }
}
