using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Sugar;
using Hudl.FFmpeg.Tests.Assets;
using Xunit;

namespace Hudl.FFmpeg.Tests.Command
{
    public class CommandFactoryTests
    {
        public CommandFactoryTests()
        {
            Utilities.SetGlobalAssets();
        }

        [Fact]
        public void Factory_AsOutput_Verify()
        {
            var factory = CommandFactory.Create();

            Assert.True(factory.Count == 0);

            factory.CreateOutputCommand();

            Assert.True(factory.Count == 1);
        }

        [Fact]
        public void Factory_AsResource_Verify()
        {
            var factory = CommandFactory.Create();

            Assert.True(factory.Count == 0);

            factory.CreateResourceCommand();

            Assert.True(factory.Count == 1);
        }

        [Fact]
        public void Factory_GetOutputEmpty_Verify()
        {
            var factory = CommandFactory.Create();

            Assert.True(factory.Count == 0);

            Assert.True(factory.GetOutputs().Count == 0);
        }

        [Fact]
        public void Factory_RenderWith_OneOutput()
        {
            var factory = CommandFactory.Create();

            factory.CreateOutputCommand()
                   .WithInput<VideoStream>(Utilities.GetVideoFile())
                   .To<Mp4>();

            var result = factory.RenderWith<TestCommandProcessor>();

            Assert.True(result.Count == 1);
        }
    }
}
