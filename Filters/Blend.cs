using System;
using System.Linq;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Filters
{
    /// <summary>
    /// Blend Video filter combines two input resources into a single Video output.
    /// </summary>
    [AppliesToResource(Type=typeof(IVideo))]
    public class Blend : BaseFilter
    {
        private const int FilterMaxInputs = 2;
        private const string FilterType = "blend";

        public Blend() 
            : base(FilterType, FilterMaxInputs)
        {
            Mode = BlendVideoModeType.and;
            Option = BlendVideoOptionType.all_expr;
        }
        public Blend(string expression) 
            : this()
        {
            Expression = expression;
        }

        public BlendVideoOptionType Option { get; set; }

        public BlendVideoModeType Mode { get; set; }

        /// <summary>
        /// the blend expression details can be found at http://ffmpeg.org/ffmpeg-all.html#blend. 
        /// </summary>
        public string Expression { get; set; }

        public override TimeSpan? LengthFromInputs(System.Collections.Generic.List<CommandResourcev2> resources)
        {
            return resources.Min(r => r.Resource.Length);
        }

        public override string ToString() 
        {
            if (Option == BlendVideoOptionType.all_expr && string.IsNullOrWhiteSpace(Expression))
            {
                throw new InvalidOperationException("Expression cannot be empty with Blend Option 'all_expr'");
            }

            var filter = new StringBuilder(100);
            filter.AppendFormat("{0}", Option.ToString());
            switch (Option) 
            {
                case BlendVideoOptionType.all_expr:
                    filter.AppendFormat("='{0}'", Expression);
                    break;
                default: 
                    filter.AppendFormat("={0}", Mode);
                    break;
            }

            return string.Concat(Type, "=", filter.ToString());
        }
    }
}
