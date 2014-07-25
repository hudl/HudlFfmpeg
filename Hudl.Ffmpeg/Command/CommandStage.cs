using System.Collections.Generic;

namespace Hudl.FFmpeg.Command
{
    public class CommandStage
    {
        internal CommandStage(FFmpegCommand stageCommand)
        {
            Command = stageCommand;
            StreamIdentifiers = new List<StreamIdentifier>();
        }

        public string LastAccessId { get; set; }

        public FFmpegCommand Command { get; set; }

        public List<StreamIdentifier> StreamIdentifiers { get; set; }

        public CommandStage Copy()
        {
            return new CommandStage(Command)
                {
                    StreamIdentifiers = StreamIdentifiers
                };
        }

        internal static CommandStage Create(FFmpegCommand stageCommand)
        {
            return new CommandStage(stageCommand);
        }
    }
}
