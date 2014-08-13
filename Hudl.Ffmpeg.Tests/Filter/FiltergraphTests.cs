using System;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Sugar;
using Hudl.FFmpeg.Tests.Assets;
using Xunit; 

namespace Hudl.FFmpeg.Tests.Filter
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
            var filterchain1 = Filterchain.FilterTo<AudioStream>();
            var filterchain2 = Filterchain.FilterTo<VideoStream>();

            filtergraph.Add(filterchain1);

            Assert.True(filtergraph.Count == 1);

            filtergraph.Add(filterchain2);

            Assert.True(filtergraph.Count == 2);
        }

        [Fact]
        public void Filtergraph_FilterTo()
        {
            var filtergraph = Filtergraph.Create(CommandHelper.CreateCommand());

            filtergraph.FilterTo<VideoStream>();

            Assert.True(filtergraph.Count == 1);

            filtergraph.FilterTo<AudioStream>();

            Assert.True(filtergraph.Count == 2);
        }

        [Fact]
        public void Filtergraph_RemoveAt()
        {
            var filtergraph = Filtergraph.Create(CommandHelper.CreateCommand());
            filtergraph.FilterTo<VideoStream>();
            filtergraph.FilterTo<VideoStream>();
            filtergraph.FilterTo<VideoStream>();

            Assert.True(filtergraph.Count == 3);

            filtergraph.RemoveAt(0);

            Assert.True(filtergraph.Count == 2);
        }

        [Fact]
        public void Filtergraph_RemoveAll()
        {
            var filtergraph = Filtergraph.Create(CommandHelper.CreateCommand());
            filtergraph.FilterTo<VideoStream>();
            filtergraph.FilterTo<VideoStream>();

            Assert.True(filtergraph.Count == 2);

            filtergraph.RemoveAll(f => true);

            Assert.True(filtergraph.Count == 0);
        }

        private class CommandHelper
        {
            private const string OutputVideo = "c:/source/output.mp4";

            public static FFmpegCommand CreateCommand()
            {
                var factory = CommandFactory.Create();

                return factory.CreateOutputCommand();
            }
        }
    }
}
