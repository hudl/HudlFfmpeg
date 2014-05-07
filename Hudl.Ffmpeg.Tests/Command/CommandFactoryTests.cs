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
        private readonly CommandConfiguration _testConfiguration = new CommandConfiguration("C:/Source/Test", "C:/Source/Ffmpeg", "C:/Source/Assets");

        [Fact]
        public void Factory_AsOutput_Verify()
        {
            var factory = new CommandFactory(_testConfiguration);

            Assert.True(factory.Count == 0);

            factory.AsOutput()
                   .WithOutput(Mp41Title); 

            Assert.True(factory.Count == 1);
        }

        [Fact]
        public void Factory_GetOutputEmpty_Verify()
        {
            var factory = new CommandFactory(_testConfiguration);

            Assert.True(factory.Count == 0);

            Assert.True(factory.GetOutput().Count == 0);
        }

        [Fact]
        public void Factory_GetOutputExport1_Verify()
        {
            var factory = new CommandFactory(_testConfiguration);

            factory.AsOutput()
                   .WithOutput(Mp42Title);

            factory.AsResource()
                   .WithOutput(Mp42Title);

            Assert.True(factory.GetOutput().Count == 2);
        }

        [Fact]
        public void Factory_GetOutputExportAll_Verify()
        {
            var factory = new CommandFactory(_testConfiguration);

            factory.AsOutput()
                   .WithOutput(Mp42Title);

            factory.AsOutput()
                   .WithOutput(Mp41Title)
                   .WithOutput(Mp42Title);

            Assert.True(factory.GetOutput().Count == 3);
        }

        [Fact]
        public void Factory_RenderWith_OneOutput()
        {
            var factory = new CommandFactory(_testConfiguration);

            factory.AsOutput()
                   .WithOutput(Mp41Title)
                   .WithInput(Utilities.GetVideoFile());
            
            var result = factory.RenderWith<TestCommandProcessor>();

            Assert.True(result.Count == 1);
        }

        [Fact]
        public void Factory_RenderWith_TwoOutput()
        {
            var factory = new CommandFactory(_testConfiguration);

            factory.AsOutput()
                   .WithOutput(Mp41Title)
                   .WithOutput(Mp42Title)
                   .WithInput(Utilities.GetVideoFile());
            
            var result = factory.RenderWith<TestCommandProcessor>(); 

            Assert.True(result.Count == 2);
        }
    }
}
