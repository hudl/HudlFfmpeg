using System.Collections.Generic;

namespace Hudl.FFmpeg.Command.BaseTypes
{
    public interface ICommandFactory
    {
        string Id { get; set; }

        List<ICommand> CommandList { get; set; }
    }
}
