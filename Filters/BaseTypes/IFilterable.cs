using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hudl.Ffmpeg.BaseTypes;

namespace Hudl.Ffmpeg.Filters.BaseTypes
{
    /// <summary>
    /// interface that forces a type to expose it's filterable interface 
    /// </summary>
    interface IFilterable
    {
        Filtergraph Filtergraph { get; set; }
    }
}
