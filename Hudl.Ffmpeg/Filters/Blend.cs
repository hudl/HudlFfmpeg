using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Metadata.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Blend Video filter combines two input resources into a single Video output.
    /// </summary>
    [ForStream(Type=typeof(VideoStream))]
    public class Blend : BaseFilter, IMetadataManipulation
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

        public override void Validate()
        {
            if (Option == BlendVideoOptionType.all_expr && string.IsNullOrWhiteSpace(Expression))
            {
                throw new InvalidOperationException("Expression cannot be empty with Blend Option 'all_expr'");
            }
        }

        public override string ToString() 
        {
            var filterParameters = new StringBuilder(100);

            switch (Option) 
            {
                case BlendVideoOptionType.all_expr:
                    FilterUtility.ConcatenateParameter(filterParameters, Formats.EnumValue(Option), Formats.EscapeString(Expression));
                    break;
                default:
                    FilterUtility.ConcatenateParameter(filterParameters, Formats.EnumValue(Option));
                    break;
            }

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }

        public virtual MetadataInfoTreeContainer EditInfo(MetadataInfoTreeContainer infoToUpdate, List<MetadataInfoTreeContainer> suppliedInfo)
        {
            infoToUpdate.VideoStream.Duration = suppliedInfo.Min(r => r.VideoStream.Duration);

            return infoToUpdate;
        }
    }
}
