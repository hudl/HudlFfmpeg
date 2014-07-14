using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Sugar;
using Hudl.Ffmpeg.Tests.Assets;
using Xunit;

namespace Hudl.Ffmpeg.Tests.Command
{
    public class CommandFactoryTests
    {
        private const string Mp41Title = "c:/source/apple.mp4";
        private const string Mp42Title = "c:/source/couch.mp4";

        public CommandFactoryTests()
        {
            Utilities.SetGlobalAssets();
        }

        [Fact]
        public void Factory_AsOutput_Verify()
        {
            var factory = CommandFactory.Create();

            Assert.True(factory.Count == 0);

            factory.CreateOutputCommand()
                   .WithOutput(Mp41Title); 

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
        public void Factory_GetOutputExport1_Verify()
        {
            var factory = CommandFactory.Create();

            factory.CreateOutputCommand()
                   .WithOutput(Mp42Title);

            factory.CreateResourceCommand()
                   .WithOutput(Mp42Title);

            Assert.True(factory.GetOutputs().Count == 2);
        }

        [Fact]
        public void Factory_GetOutputExportAll_Verify()
        {
            var factory = CommandFactory.Create();

            factory.CreateOutputCommand()
                   .WithOutput(Mp42Title);

            factory.CreateOutputCommand()
                   .WithOutput(Mp41Title)
                   .WithOutput(Mp42Title);

            Assert.True(factory.GetOutputs().Count == 3);
        }

        [Fact]
        public void Factory_RenderWith_OneOutput()
        {
            var factory = CommandFactory.Create();

            factory.CreateOutputCommand()
                   .WithOutput(Mp41Title)
                   .WithInput(Utilities.GetVideoFile());
            
            var result = factory.RenderWith<TestCommandProcessor>();

            Assert.True(result.Count == 1);
        }

        [Fact]
        public void Factory_RenderWith_TwoOutput()
        {
            var factory = CommandFactory.Create();

            factory.CreateOutputCommand()
                   .WithOutput(Mp41Title)
                   .WithOutput(Mp42Title)
                   .WithInput(Utilities.GetVideoFile());
            
            var result = factory.RenderWith<TestCommandProcessor>(); 

            Assert.True(result.Count == 2);
        }
    }
}
