using System.Collections.Generic;

namespace Hudl.Ffmpeg.Command
{
    public class CommandStage
    {
        internal CommandStage(FfmpegCommand stageCommand)
        {
            Command = stageCommand;
            StreamIdentifiers = new List<StreamIdentifier>();
        }

        public string LastAccessId { get; set; }

        public FfmpegCommand Command { get; set; }

        public List<StreamIdentifier> StreamIdentifiers { get; set; }

        public CommandStage Copy()
        {
            return new CommandStage(Command)
                {
                    StreamIdentifiers = StreamIdentifiers
                };
        }

        internal static CommandStage Create(FfmpegCommand stageCommand)
        {
            return new CommandStage(stageCommand);
        }
    }
}
