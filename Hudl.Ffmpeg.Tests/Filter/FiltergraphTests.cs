using System;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Sugar;
using Hudl.Ffmpeg.Tests.Assets;
using Xunit; 

namespace Hudl.Ffmpeg.Tests.Filter
{
    public class FiltergraphTests
    {
        public FiltergraphTests()
        {
            Utilities.SetGlobalAssets();
        }

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

            public static FfmpegCommand CreateCommand()
            {
                var factory = CommandFactory.Create();

                return factory.CreateOutputCommand()
                              .WithOutput(OutputVideo);
            }
        }
    }
}
