using System;
using System.IO;
using Hudl.Ffmpeg.Command.BaseTypes;

namespace Hudl.Ffmpeg.Command
{
    public class WinCmdProcessorReciever : ICommandProcessor
    {
        private StreamWriter _outputWriter;

        public WinCmdProcessorReciever()
        {
            Status = CommandProcessorStatus.Closed;
        }

        private CommandConfiguration Configuration { get; set; }

        public Exception Error { get; protected set; }

        public CommandProcessorStatus Status { get; protected set; }

        public bool Open(CommandConfiguration configuration)
        {
            if (Status != CommandProcessorStatus.Closed)
            {
                throw new InvalidOperationException(string.Format("Cannot open a command processor that is currently in the '{0}' state.", Status));
            }

            Configuration = configuration;

            try
            {
                _outputWriter = new StreamWriter(Path.Combine(configuration.OutputPath, Guid.NewGuid() + ".bat"));
                Status = CommandProcessorStatus.Ready;
            }
            catch (Exception err)
            {
                Error = err;
                Status = CommandProcessorStatus.Faulted;
                return false;
            }

            //write the commands for preparation 
            WritePreparation();

            return true;
        }

        public bool Close()
        {
            if (Status != CommandProcessorStatus.Ready)
            {
                throw new InvalidOperationException(string.Format("Cannot close a command processor that is currently in the '{0}' state.", Status));
            }

            //write the command for clean up
            WriteCleanUp();
            
            try
            {
                _outputWriter.Flush();
                _outputWriter.Close();
                _outputWriter.Dispose();
                Status = CommandProcessorStatus.Closed;
            }
            catch (Exception err)
            {
                Error = err;
                Status = CommandProcessorStatus.Faulted;
                return false;
            }
            return true;
        }

        public bool Send(string command)
        {
            if (Status != CommandProcessorStatus.Ready)
            {
                throw new InvalidOperationException(string.Format("Cannot process a command processor that is currently in the '{0}' state.", Status));
            }
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentException("Processing command cannot be null or empty.", "command");
            }

            try
            {
                Status = CommandProcessorStatus.Processing;

                _outputWriter.WriteLine(command);

                Status = CommandProcessorStatus.Ready;
            }
            catch (Exception err)
            {
                Error = err;
                Status = CommandProcessorStatus.Faulted;
                return false;
            }
            return true;
        }

        private void WritePreparation()
        {
            Send("cd " + Configuration.FfmpegPath);

            Send("mkdir " + Configuration.TempPath);

            Send("mkdir " + Configuration.OutputPath);
        }

        private void WriteCleanUp()
        {
            Send("rmdir " + Configuration.TempPath);
        }
    }
}
