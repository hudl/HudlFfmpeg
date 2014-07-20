using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources.BaseTypes;

namespace Hudl.Ffmpeg.Tests.Filter.Identify
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
