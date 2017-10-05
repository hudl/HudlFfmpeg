using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFprobe;
using Hudl.FFprobe.Command;
using Hudl.FFprobe.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hudl.FFmpeg.Tests.Command
{
    public class FFprobeComandTests
    {
        public FFprobeComandTests()
        {
            Assets.Utilities.SetGlobalAssets();
        }

        public IContainer GetVideoContainer()
        {
            var videoUrl = Assets.Utilities.GetVideoFile();
            return Resource.From(videoUrl);
        }

        public IContainer GetAudioContainer()
        {
            var audioUrl = Assets.Utilities.GetAudioFile();
            return Resource.From(audioUrl);
        }

        public FFprobeCommand GetCommand()
        {
            var container = GetVideoContainer();
            return FFprobeCommand.Create(container); 
        }

        public MediaLoader GetMediaLoader()
        {
            var container = GetVideoContainer();
            return new MediaLoader(container); 
        }

        [Fact]
        public void FFprobeCommand_AddSettings_Verify()
        {
            var command = GetCommand();

            Assert.Throws<ArgumentNullException>(() => command.AddSetting(null));
            command.AddSetting(new PrintFormat("json"));
        }

        [Fact]
        public void FFprobeCommand_LoadSettings_Verify()
        {
            var videoContainer = GetVideoContainer();
            var audioContainer = GetAudioContainer();
            var loader = GetMediaLoader();

            loader.ReadInfo(videoContainer, MediaLoader.LoaderFlags.ShowFormat);
            loader.ReadInfo(videoContainer, MediaLoader.LoaderFlags.ShowStreams);
            loader.ReadInfo(videoContainer, MediaLoader.LoaderFlags.ShowFrames);

            loader.ReadInfo(audioContainer, MediaLoader.LoaderFlags.ShowFormat);
            loader.ReadInfo(audioContainer, MediaLoader.LoaderFlags.ShowStreams);
            loader.ReadInfo(audioContainer, MediaLoader.LoaderFlags.ShowFrames);
        }
    }
}
