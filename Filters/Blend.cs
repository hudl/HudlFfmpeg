using System;
using System.Linq;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
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
            Mode = BlendVideoModeTypes.and;
            Option = BlendVideoOptionTypes.all_expr;
        }
        public Blend(string expression) 
            : this()
        {
            Expression = expression;
        }

        public BlendVideoOptionTypes Option { get; set; }

        public BlendVideoModeTypes Mode { get; set; }

        public string Expression { get; set; }

        public override TimeSpan? LengthFromInputs(System.Collections.Generic.List<Command.CommandResource<IResource>> resources)
        {
            return resources.Min(r => r.Resource.Length);
        }

        public override string ToString() 
        {
            if (Option == BlendVideoOptionTypes.all_expr && string.IsNullOrWhiteSpace(Expression))
            {
                throw new InvalidOperationException("Expression cannot be empty with Blend Option 'all_expr'");
            }

            var filter = new StringBuilder(100);
            filter.AppendFormat("{0}", Option.ToString());
            switch (Option) 
            {
                case BlendVideoOptionTypes.all_expr:
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
