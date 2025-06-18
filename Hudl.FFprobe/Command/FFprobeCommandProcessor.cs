using System;
using System.Diagnostics;
using System.IO;
using Hudl.FFmpeg;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Exceptions;
using Hudl.FFmpeg.Logging;

namespace Hudl.FFprobe.Command;

internal class FFprobeCommandProcessor : ICommandProcessor
{
    private static readonly LogUtility Log = LogUtility.GetLogger(typeof(FFprobeCommandProcessor));

    public FFprobeCommandProcessor()
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
        return Send(command, null);
    }

    public bool Send(string command, int? timeoutMilliseconds)
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

        try
        {
            Status = CommandProcessorStatus.Processing;

            ProcessIt(command, timeoutMilliseconds);

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

    private void ProcessIt(string command, int? timeoutMilliseconds)
    {
        using (var FFprobeProcess = new Process())
        {
            FFprobeProcess.StartInfo = new ProcessStartInfo
            {
                FileName = ResourceManagement.CommandConfiguration.FFprobePath,
                WorkingDirectory = ResourceManagement.CommandConfiguration.TempPath,
                Arguments = command.Trim(),
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };

            Log.DebugFormat("FFprobe.exe Args={0}.", FFprobeProcess.StartInfo.Arguments);
                
            FFprobeProcess.Start();
                
            StdOut = FFprobeProcess.StandardOutput.ReadToEnd();

            FFprobeProcess.WaitForExit();

            Log.DebugFormat("FFprobe.exe Output={0}.", StdOut);

            var exitCode = FFprobeProcess.ExitCode;
            if (exitCode != 0)
            {
                throw new FFmpegProcessingException(exitCode, StdOut);
            }
        }
    }
}