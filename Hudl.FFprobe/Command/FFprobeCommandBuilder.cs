using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Command.Models;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.Interfaces;
using Hudl.FFmpeg.Settings.Serialization;

namespace Hudl.FFprobe.Command;

internal class FFprobeCommandBuilder : FFCommandBuilderBase, ICommandBuilder
{
    public void WriteCommand(ICommand command)
    {
        WriteCommand((FFprobeCommand)command);
    }

    public void WriteCommand(FFprobeCommand command)
    {
        var inputResource = new Input(command.Resource);
        BuilderBase.Append(" ");
        BuilderBase.Append(SettingSerializer.Serialize(inputResource));

        command.Settings.ForEach(WriteSerializerSpecifier);
    }
    public void WriteSerializerSpecifier(ISetting setting)
    {
        BuilderBase.Append(" ");
        BuilderBase.Append(SettingSerializer.Serialize(setting));
    }
}