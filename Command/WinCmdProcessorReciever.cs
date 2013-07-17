using System;
using System.Diagnostics;
using System.IO;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command.BaseTypes;
using log4net;

namespace Hudl.Ffmpeg.Command
{
    public class WinCmdProcessorReciever : ICommandProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WinCmdProcessorReciever).Name);

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


            try
            {
                Log.DebugFormat("Opening command processor.");

                Configuration = configuration;

                Create();

                Status = CommandProcessorStatus.Ready;
            }
            catch (Exception err)
            {
                Error = err;
                Status = CommandProcessorStatus.Faulted;
                return false;
            }

            //write the commands for preparation 

            return true;
        }

        public bool Close()
        {
            if (Status != CommandProcessorStatus.Ready)
            {
                throw new InvalidOperationException(string.Format("Cannot close a command processor that is currently in the '{0}' state.", Status));
            }

            try
            {
                Log.DebugFormat("Closing command processor.");

                Delete();

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

                ProcessIt(command);

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

        private void Create()
        {
            if (Configuration.RunSetup)
            {
                Log.DebugFormat("Creating temporary directories.");

                Directory.CreateDirectory(Configuration.TempPath);

                Directory.CreateDirectory(Configuration.OutputPath);
            }
        }

        private void Delete()
        {
            if (Configuration.RunCleanup)
            {
                Log.DebugFormat("Removing temporary directories.");

                Directory.Delete(Configuration.TempPath, true);
            }
        }

        private void ProcessIt(string command)
        {
            using (var ffmpegProcess = new Process())
            {
                ffmpegProcess.StartInfo = new ProcessStartInfo()
                {
                    FileName = Configuration.FfmpegPath,
                    WorkingDirectory = Configuration.TempPath,
                    Arguments = command.Trim(),
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                };

                Log.DebugFormat("ffmpeg.exe Args={0}.", ffmpegProcess.StartInfo.Arguments);
                
                ffmpegProcess.Start();

                var errorOutput = ffmpegProcess.StandardError.ReadToEnd();

                ffmpegProcess.WaitForExit();

                Log.DebugFormat("ffmpeg.exe Output={0}.", errorOutput);

                var exitCode = ffmpegProcess.ExitCode;
                if (exitCode != 0)
                {
                    throw new FfmpegProcessingException(exitCode, errorOutput);
                }
            }
        }
    }
}
