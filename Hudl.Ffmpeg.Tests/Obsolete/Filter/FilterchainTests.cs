using System;
using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Filters.Obsolete;
using Hudl.Ffmpeg.Filters.Obsolete.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Xunit; 

namespace Hudl.Ffmpeg.Tests.Obsolete.Filter
{
    public class FilterchainTests
    {
        [Fact]
        public void FilterchainIVideo_Add_Restriction()
        {
            var filterchain = Filterchain.FilterTo<Mp4>();
            Assert.DoesNotThrow(() => filterchain.Filters.Add(new Concat()));
            Assert.Throws<AppliesToInvalidException>(() => filterchain.Filters.Add(new AMix()));
        }

        [Fact]
        public void FilterchainIAudio_Add_Restriction()
        {
            var filterchain = Filterchain.FilterTo<Mp3>();
            Assert.DoesNotThrow(() => filterchain.Filters.Add(new Concat()));
            Assert.Throws<AppliesToInvalidException>(() => filterchain.Filters.Add(new Fade()));
        }

        [Fact]
        public void FilterchainIImage_Add_Restriction()
        {
            var filterchain = Filterchain.FilterTo<Png>();
            Assert.DoesNotThrow(() => filterchain.Filters.Add(new Overlay()));
            Assert.Throws<AppliesToInvalidException>(() => filterchain.Filters.Add(new Fade()));
        }

        [Fact]
        public void Filterchain_Add_Duplicate()
        {
            var filterchain = Filterchain.FilterTo<Png>();
            Assert.DoesNotThrow(() => filterchain.Filters.Add(new Overlay()));
            Assert.Throws<InvalidOperationException>(() => filterchain.Filters.Add(new Overlay()));
        }

        [Fact]
        public void FilterchainIVideo_RemoveAt()
        {
            var filterchain = Filterchain.FilterTo<Mp4>(
                new Fade(), 
                new ColorBalance());

            Assert.True(filterchain.Filters.Count == 2);

            filterchain.Filters.RemoveAt(0);

            Assert.True(filterchain.Filters.Count == 1);
        }

        [Fact]
        public void FilterchainIVideo_RemoveType()
        {
            var filterchain = Filterchain.FilterTo<Mp4>(
                new Fade(),
                new ColorBalance());

            Assert.True(filterchain.Filters.Contains<Fade>());

            filterchain.Filters.Remove<Fade>();

            Assert.False(filterchain.Filters.Contains<Fade>());
        }

        [Fact]
        public void FilterchainIVideo_RemoveAll()
        {
            var filterchain = Filterchain.FilterTo<Mp4>(
                new Fade(),
                new ColorBalance());

            Assert.True(filterchain.Filters.Count == 2);

            filterchain.Filters.RemoveAll(filter => true);

            Assert.True(filterchain.Filters.Count == 0);
        }

    }
}
