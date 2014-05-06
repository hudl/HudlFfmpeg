using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.Obsolete;
using Hudl.Ffmpeg.Filters.Obsolete.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Xunit; 

namespace Hudl.Ffmpeg.Tests.Obsolete.Filter
{
    public class FiltergraphTests
    {
        [Fact]
        public void Filtergraph_EmptyToString_ThrowsException()
        {
            var filtergraph = new Filtergraph();

            Assert.Throws<InvalidOperationException>(() => filtergraph.ToString());
        }

        [Fact]
        public void Filtergraph_Add()
        {
            var filtergraph = new Filtergraph();
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
            var filtergraph = new Filtergraph();

            filtergraph.FilterTo<Mp4>();

            Assert.True(filtergraph.Count == 1);

            var outputMp4 = new Mp4();
            filtergraph.FilterTo(outputMp4);

            Assert.True(filtergraph.Count == 2);
        }

        [Fact]
        public void Filtergraph_RemoveAt()
        {
            var filtergraph = new Filtergraph();
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
            var filtergraph = new Filtergraph();
            filtergraph.FilterTo<Mp4>();
            filtergraph.FilterTo<Mp4>();

            Assert.True(filtergraph.Count == 2);

            filtergraph.RemoveAll(f => true);

            Assert.True(filtergraph.Count == 0);
        }
    }
}
