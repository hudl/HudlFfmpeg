using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Sugar;
using Hudl.Ffmpeg.Tests.Assets;
using Xunit;

namespace Hudl.Ffmpeg.Tests.Command
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

        [Fact]
        public void Factory_GetOutputs_Verify()
        {
            var factory = CommandFactory.Create();

            factory.CreateResourceCommand();

            Assert.True(factory.GetOutputs().Count == 0);

            factory.CreateOutputCommand();

            Assert.True(factory.GetOutputs().Count == 1);
        }

        [Fact]
        public void Factory_GetResources_Verify()
        {
            var factory = CommandFactory.Create();

            factory.CreateResourceCommand();

            Assert.True(factory.GetResources().Count == 1);

            factory.CreateOutputCommand();

            Assert.True(factory.GetResources().Count == 1);
        }
    }
}
