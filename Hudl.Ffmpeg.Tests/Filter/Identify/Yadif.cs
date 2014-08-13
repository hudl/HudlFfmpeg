using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hudl.FFmpeg.BaseTypes;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Tests.Filter.Identify
{
    [ForStream(Type = typeof(VideoStream))]
    public class Yadif : BaseFilter
    {
        private const int FilterMaxInputs = 1;
        private const string FilterType = "yadif";

        public Yadif()
            : base(FilterType, FilterMaxInputs)
        {
        }

        public override string ToString() 
        {
            var filterParameters = new StringBuilder(100);

            FilterUtility.ConcatenateParameter(filterParameters, "1"); 

            return FilterUtility.JoinTypeAndParameters(this, filterParameters);
        }
    }
}
