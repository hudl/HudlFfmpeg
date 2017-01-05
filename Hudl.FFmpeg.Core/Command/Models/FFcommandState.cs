using System;
using System.Linq;
using System.Collections.Generic;
using Hudl.FFmpeg.Command.BaseTypes;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.FFmpeg.Command.Models
{

    public enum FFcommandState
    {
        Started = 0, 
        Stopped = 1, 
        Faulted = 2
    }

}