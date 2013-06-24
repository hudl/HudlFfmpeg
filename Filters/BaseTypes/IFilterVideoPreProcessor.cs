using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes; 

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    /// <summary>
    /// complex filters and templates may require video to be in a specific format, or for inputs to be split appropriately
    /// Filter preprocessors allow that to happen by givin gthe filter access to the project resources for manipulation. 
    /// </summary>
    interface IFilterVideoPreProcessor
    {
    }
}
