using Hudl.FFmpeg.Command.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hudl.FFmpeg.Command.Models
{
    public class FFcommandContext
    {
        public FFcommandState State { get; set; }

        public ICommandFactory Factory { get; set; }

        public ICommand Command { get; set; }

        public string Arguments { get; set; }

        public string StdErr { get; set; }
    }
}
