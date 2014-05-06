using Hudl.Ffmpeg.Command.Obsolete;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Tests.Command;
using Xunit;

namespace Hudl.Ffmpeg.Tests.Obsolete.Command
{
    public class CommandFactoryTests
    {
        private readonly Ffmpeg.Command.CommandConfiguration TestConfiguration = new Ffmpeg.Command.CommandConfiguration("C:/Source/Test", "C:/Source/Ffmpeg", "C:/Source/Assets");

        [Fact]
        public void Factory_OutputAs_Verify()
        {
            var factory = new CommandFactory(TestConfiguration);

            Assert.True(factory.Count == 0);

            factory.AddToOutput(factory.CreateOutput<Mp4>());

            Assert.True(factory.Count == 1);
        }

        [Fact]
        public void Factory_GetOutputEmpty_Verify()
        {
            var factory = new CommandFactory(TestConfiguration);

            Assert.True(factory.Count == 0);

            Assert.True(factory.GetOutput().Count == 0);
        }

        [Fact]
        public void Factory_GetOutputExport1_Verify()
        {
            var factory = new CommandFactory(TestConfiguration);

            factory.AddToOutput(factory.CreateOutput<Mp4>());
            factory.AddToResources(factory.CreateOutput<Mp4>());

            Assert.True(factory.GetOutput().Count == 1);
        }

        [Fact]
        public void Factory_GetOutputExportAll_Verify()
        {
            var factory = new CommandFactory(TestConfiguration);

            factory.AddToOutput(factory.CreateOutput<Mp4>());
            factory.AddToOutput(factory.CreateOutput<Mp4>());

            Assert.True(factory.GetOutput().Count == 2);
        }

        [Fact]
        public void Factory_RenderWith()
        {
            var factory = new CommandFactory(TestConfiguration);
            var command = factory.CreateOutput<Mp4>();
            command.AddResource<Mp4>("c:/source/apples.mp4");
            factory.AddToOutput(command);
            var result = factory.RenderWith<TestCommandProcessor>(); 

            Assert.True(result.Count == 1);
        }
    }
}
