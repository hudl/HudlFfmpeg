using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Command.BaseTypes;
using Hudl.FFmpeg.Command.Models;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFmpeg.Settings.Interfaces;

namespace Hudl.FFprobe.Command;

public class FFprobeCommand : FFCommandBase
{
    private FFprobeCommand(IContainer resource)
    {
        Resource = resource;
        Settings = new List<ISetting>();
    }

    public IContainer Resource { get; set; }

    public List<ISetting> Settings { get; set; }

    public static FFprobeCommand Create(IContainer resource)
    {
        return new FFprobeCommand(resource);    
    }

    public FFprobeCommand AddSetting(ISetting setting)
    {
        if (setting == null)
        {
            throw new ArgumentNullException("setting");
        }

        Settings.Add(setting);

        return this;
    }

    public ICommandProcessor Execute()
    {
        return Execute(null);
    }

    public ICommandProcessor Execute(int? timeoutMilliseconds)
    {
        return ExecuteWith<FFprobeCommandProcessor, FFprobeCommandBuilder>(timeoutMilliseconds);
    }

}