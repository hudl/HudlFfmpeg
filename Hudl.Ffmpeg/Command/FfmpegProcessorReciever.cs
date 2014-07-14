using System;
using System.Diagnostics;
using System.IO;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Command.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Logging;
using Hudl.Ffmpeg.Sugar;

namespace Hudl.Ffmpeg.Command
{
    internal class FfmpegProcessorReciever : ICommandProcessor
    {
        private static readonly LogUtility Log = LogUtility.GetLogger(typeof(FfmpegProcessorReciever));

        private const int MaximumRetryFailures = 1; 

        public FfmpegProcessorReciever()
        {
            Status = CommandProcessorStatus.Closed;
        }

        public Exception Error { get; protected set; }

        public string StdOut { get; protected set; }

        public CommandProcessorStatus Status { get; protected set; }

        public bool Open()
        {
            if (Status != CommandProcessorStatus.Closed)
            {
                throw new InvalidOperationException(string.Format("Cannot open a command processor that is currently in the '{0}' state.", Status));
            }

            try
            {
                Log.SetAttributes(ResourceManagement.CommandConfiguration.LoggingAttributes);

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
            if (ResourceManagement.CommandConfiguration.HasFlag(CommandConfigurationFlagTypes.PerformPreRenderSetup))
            {
                Log.DebugFormat("Creating temporary directories.");

                Directory.CreateDirectory(ResourceManagement.CommandConfiguration.TempPath);

                Directory.CreateDirectory(ResourceManagement.CommandConfiguration.OutputPath);
            }
        }

        private void Delete()
        {
            if (ResourceManagement.CommandConfiguration.HasFlag(CommandConfigurationFlagTypes.PerformPostRenderCleanup))
            {
                Log.DebugFormat("Removing temporary directories.");

                Directory.Delete(ResourceManagement.CommandConfiguration.TempPath, true);
            }
        }

        private void ProcessIt(string command)
        {
            using (var ffmpegProcess = new Process())
            {
                ffmpegProcess.StartInfo = new ProcessStartInfo
                {
                    FileName = ResourceManagement.CommandConfiguration.FfmpegPath,
                    WorkingDirectory = ResourceManagement.CommandConfiguration.TempPath,
                    Arguments = command.Trim(),
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                };

                Log.DebugFormat("ffmpeg.exe Args={0}.", ffmpegProcess.StartInfo.Arguments);
                
                ffmpegProcess.Start();

                StdOut = ffmpegProcess.StandardError.ReadToEnd();

                ffmpegProcess.WaitForExit();

                Log.DebugFormat("ffmpeg.exe Output={0}.", StdOut);

                var exitCode = ffmpegProcess.ExitCode;
                if (exitCode != 0)
                {
                    throw new FfmpegProcessingException(exitCode, StdOut);
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
            if (ResourceManagement.CommandConfiguration.HasFlag(CommandConfigurationFlagTypes.RetrySignal15Termination) && IsSignal15Error(errorOutput))
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
