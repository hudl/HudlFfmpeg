using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Filters.Attributes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;

namespace Hudl.FFmpeg.Filters
{
    /// <summary>
    /// Apply color adjustments using curves.
    /// </summary>
    [ForStream(Type = typeof(VideoStream))]
    [Filter(Name = "curves", MinInputs = 1, MaxInputs = 1)]
    public class Curves : IFilter
    {
        public Curves()
        {
        }
        public Curves(string expression)
            : this()
        {
            Expression = expression;
        }

        [FilterParameter]
        public string Expression { get; set; }
    }
}
