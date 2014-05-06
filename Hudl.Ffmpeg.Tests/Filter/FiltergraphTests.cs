using System;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Sugar;
using Xunit; 

namespace Hudl.Ffmpeg.Tests.Filter
{
    public class FiltergraphTests
    {
        [Fact]
        public void Filtergraph_EmptyToString_ThrowsException()
        {

            var filtergraph = Filtergraph.Create(CommandHelper.CreateCommand()); 

            Assert.Throws<InvalidOperationException>(() => filtergraph.ToString());
        }

        [Fact]
        public void Filtergraph_Add()
        {
            var filtergraph = Filtergraph.Create(CommandHelper.CreateCommand());
            var filterchain1 = Filterchain.FilterTo<Mp4>();
            var filterchain2 = Filterchain.FilterTo<Mp4>();

            filtergraph.Add(filterchain1);

            Assert.True(filtergraph.Count == 1);

            filtergraph.Add(filterchain2);

            Assert.True(filtergraph.Count == 2);
        }

        [Fact]
        public void Filtergraph_FilterTo()
        {
            var filtergraph = Filtergraph.Create(CommandHelper.CreateCommand());

            filtergraph.FilterTo<Mp4>();

            Assert.True(filtergraph.Count == 1);

            var outputMp4 = new Mp4();
            filtergraph.FilterTo(outputMp4);

            Assert.True(filtergraph.Count == 2);
        }

        [Fact]
        public void Filtergraph_RemoveAt()
        {
            var filtergraph = Filtergraph.Create(CommandHelper.CreateCommand());
            filtergraph.FilterTo<Mp4>();
            filtergraph.FilterTo<Mp4>();
            filtergraph.FilterTo<Mp4>();

            Assert.True(filtergraph.Count == 3);

            filtergraph.RemoveAt(0);

            Assert.True(filtergraph.Count == 2);
        }

        [Fact]
        public void Filtergraph_RemoveAll()
        {
            var filtergraph = Filtergraph.Create(CommandHelper.CreateCommand());
            filtergraph.FilterTo<Mp4>();
            filtergraph.FilterTo<Mp4>();

            Assert.True(filtergraph.Count == 2);

            filtergraph.RemoveAll(f => true);

            Assert.True(filtergraph.Count == 0);
        }

        private class CommandHelper
        {
            private const string OutputVideo = "c:/source/output.mp4";
            private static readonly CommandConfiguration _testConfiguration = new CommandConfiguration("C:/Source/Test", "C:/Source/Ffmpeg", "C:/Source/Assets");

            public static FfmpegCommand CreateCommand()
            {
                var factory = new CommandFactory(_testConfiguration);

                return factory.AsOutput()
                              .WithOutput(OutputVideo);
            }

            public static FfmpegCommand CreateCommandNoOut()
            {
                var factory = new CommandFactory(_testConfiguration);

                return factory.AsOutput();
            }

        }
    }
}
