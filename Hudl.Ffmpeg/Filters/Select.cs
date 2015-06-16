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
    /// select filter selects unfiltered video frames and passes them along to the output stream 
    /// </summary>
    [AppliesToResource(Type=typeof(IVideo))]
    public class Select : BaseFilter
    {
        // this filter does modify the output metadata, however it does so based on an  expression. 
        // in order to correctly determine the metadata in the output stream we will need to create
        // an expression parser so that we can run through the possible values and correctly set the 
        // output stream info. this will be done in current version upon merging this code with 
        // Hudl.FFmpeg 3.0 

        const int FilterMaxInputs = 1;
        private const string FilterType = "select";

        public Select() 
            : base(FilterType, FilterMaxInputs)
        {
        }
        public Select(string expression) 
            : this()
        {
            Expression = expression;
        }

        /// <summary>
        /// the select expression details can be found at https://ffmpeg.org/ffmpeg-filters.html#select_002c-aselect
        /// </summary>
        public string Expression { get; set; }

        public override string ToString()
        {
            return string.Concat(Type, "='", Expression, "'");
        }
    }
}
