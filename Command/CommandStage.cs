using System.Collections.Generic;

namespace Hudl.Ffmpeg.Command
{
    public class CommandStage
    {
        internal CommandStage(FfmpegCommand stageCommand)
        {
            Command = stageCommand;
            Receipts = new List<CommandReceipt>();
        }

        public FfmpegCommand Command { get; set; }

        public List<CommandReceipt> Receipts { get; set; }
    }
}
