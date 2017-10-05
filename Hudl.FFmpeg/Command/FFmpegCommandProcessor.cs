using System;
using System.Diagnostics;
using System.IO;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Exceptions;
using Hudl.FFmpeg.Logging;
using Hudl.FFmpeg.Command.StreamReaders;
using System.Threading;
using Hudl.FFmpeg.Extensions;
using System.Threading.Tasks;

namespace Hudl.FFmpeg.Command
{
    internal class FFmpegCommandProcessor : ICommandProcessor
    {
        private static readonly LogUtility Log = LogUtility.GetLogger(typeof(FFmpegCommandProcessor));

        private const int MaximumRetryFailures = 1;

        public FFmpegCommandProcessor()
        {
            Status = CommandProcessorStatus.Closed;
        }

        public Exception Error { get; protected set; }

        public string StdOut { get; protected set; }
        public string Command { get; protected set; }

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
            return Send(command, default(CancellationToken));
        }

        public bool Send(string command, CancellationToken token = default(CancellationToken))
        {
            if (Status != CommandProcessorStatus.Ready)
            {
                throw new InvalidOperationException(string.Format("Cannot process a command processor that is currently in the '{0}' state.", Status));
            }
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentException("Processing command cannot be null or empty.", "command");
            }

            Command = command; 

            var retryCount = 0; 
            var isSuccessful = false;
            do
            {
                try
                {
                    Status = CommandProcessorStatus.Processing;

                    ProcessIt(command, token);

                    Status = CommandProcessorStatus.Ready;

                    isSuccessful = true;
                }
                catch (TaskCanceledException)
                {
                    throw;
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

        private void ProcessIt(string command, CancellationToken token = default(CancellationToken))
        {
            using (var ffmpegProcess = new Process())
            {
                ffmpegProcess.StartInfo = new ProcessStartInfo
                {
                    FileName = ResourceManagement.CommandConfiguration.FFmpegPath,
                    WorkingDirectory = ResourceManagement.CommandConfiguration.TempPath,
                    Arguments = command.Trim(),
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                };

                Log.Debug($"ffmpeg.exe MonoRuntime={ResourceManagement.IsMonoRuntime()} Args={ffmpegProcess.StartInfo.Arguments}");

                var stdErrorReader = StandardErrorAsyncStreamReader.AttachReader(ffmpegProcess);

                using (var registration = token.Register(() => ffmpegProcess.Kill()))
                {
                    ffmpegProcess.Start();

                    //workaround for a bug in the mono process when attempting to read async from console output events 
                    //   - link http://mono.1490590.n4.nabble.com/System-Diagnostic-Process-and-event-handlers-td3246096.html
                    // we will wait a total of 10 seconds for the process to start, if nothing has happened in that time then we will 
                    // return a failure for the event. 
                    ffmpegProcess.WaitForProcessStart();

                    stdErrorReader.Listen();

                    var processStopped = ffmpegProcess.WaitForProcessStop();
                    if (!processStopped)
                    {
                        throw new FFmpegTimeoutException(ffmpegProcess.StartInfo.Arguments);
                    }

                    stdErrorReader.Stop();

                    token.ThrowIfCancellationRequested();

                    StdOut = stdErrorReader.ToString();
                }

                Log.Debug($"ffmpeg.exe MonoRuntime={ResourceManagement.IsMonoRuntime()}  Output={StdOut}.");

                var exitCode = ffmpegProcess.ExitCode;
                if (exitCode != 0)
                {
                    throw new FFmpegProcessingException(exitCode, StdOut);
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
                Log.Warn("FFmpeg has encountered a Signal 15 Terminating error."); 
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
