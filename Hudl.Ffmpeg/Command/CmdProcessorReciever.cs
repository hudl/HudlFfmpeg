using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Logging;
using Hudl.Ffmpeg.Sugar;

namespace Hudl.Ffmpeg.Command
{
    public class CmdProcessorReciever : ICommandProcessor
    {
        private static readonly LogUtility Log = LogUtility.GetLogger(typeof(CmdProcessorReciever));

        private const int MaximumRetryFailures = 1; 

        public CmdProcessorReciever()
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
                Configuration = configuration;

                Log.SetAttributes(configuration.LoggingAttributes);

                Log.DebugFormat("Opening command processor.");

                Create();

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

            var retryCount = 0; 
            var isSuccessful = false;
            do
            {
                try
                {
                    Status = CommandProcessorStatus.Processing;

                    ProcessIt(command);

                    Status = CommandProcessorStatus.Ready;

                    isSuccessful = true;
                }
                catch (Exception err)
                {
                    if (!CheckForKnownExceptions(err))
                    {
                        Error = err;
                        Status = CommandProcessorStatus.Faulted;
                        return false;
                    }

                    retryCount++; 
                }
            } while (!isSuccessful && retryCount <= MaximumRetryFailures);

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
                ffmpegProcess.StartInfo = new ProcessStartInfo
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

        private bool CheckForKnownExceptions(Exception err)
        {
            var errorOutput = err.Data.Contains("ErrorOutput") 
                ? err.Data["ErrorOutput"].ToString() 
                : string.Empty; 

            if (string.IsNullOrWhiteSpace(errorOutput))
            {
                return false; 
            }
            
            //signal 15 terminating: 
            // this happens when ffmpeg times out during the transcode, when we have encountered these in the field
            // a reingest has fixed them. The ffmpeg error output indicates no error from the decoding the stream 
            // ther than that which was indicated above.
            if (Configuration.Has(CommandConfigurationFlagTypes.RetrySignal15Termination) && IsSignal15Error(errorOutput))
            {
                Log.Warn("Ffmpeg has encountered a Signal 15 Terminating error."); 
                return true;
            }

            //by default we will not retry any commands. 
            return false;
        }

#region Error Output Checks
        private const string ErrorSignal15Terminating = "Received signal 15: terminating"; 

        private static bool IsSignal15Error(string errorOutput)
        {
            var errorIndex = errorOutput.IndexOf(ErrorSignal15Terminating, StringComparison.InvariantCulture);
            return errorIndex > -1; 
        }

#endregion 

    }
}
