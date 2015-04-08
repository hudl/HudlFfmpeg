using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Command.Models;
using Hudl.FFmpeg.Settings;
using Hudl.FFprobe.Options.BaseTypes;

namespace Hudl.FFprobe.Command
{
    internal class FFprobeCommandBuilder : FFcommandBuilderBase, ICommandBuilder
    {
        public void WriteCommand(ICommand command)
        {
            WriteCommand((FFprobeCommand)command);
        }

        public void WriteCommand(FFprobeCommand command)
        {
            var inputResource = new Input(command.Resource);
            BuilderBase.Append(" ");
            BuilderBase.Append(inputResource);

            command.Options.ForEach(WriteSerializerSpecifier);
        }
        public void WriteSerializerSpecifier(IFFprobeOptions serializer)
        {
            BuilderBase.Append(" ");
            BuilderBase.Append(serializer.Setting);
        }
    }
}
