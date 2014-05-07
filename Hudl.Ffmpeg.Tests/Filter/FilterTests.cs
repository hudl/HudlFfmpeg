using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Filters.Templates;
using Hudl.Ffmpeg.Sugar;
using Hudl.Ffmpeg.Templates;
using Xunit;

namespace Hudl.Ffmpeg.Tests.Filter
{
    public class FilterTests
    {
        #region Empty
        [Fact]
        public void AFade_EmptyToString_ThrowsException()
        {
            var filter = FilterFactory.CreateEmpty<AFade>();

            Assert.Throws<InvalidOperationException>(() => filter.ToString());
        }

        [Fact]
        public void AMix_EmptyToString_ThrowsException()
        {
            var filter = FilterFactory.CreateEmpty<AMix>();

            Assert.Throws<InvalidOperationException>(() => filter.ToString());
        }

        [Fact]
        public void AMovie_EmptyToString_ThrowsException()
        {
            var filter = FilterFactory.CreateEmpty<AMovie>();

            Assert.Throws<InvalidOperationException>(() => filter.ToString());
        }

        [Fact]
        public void Blend_EmptyToString_ThrowsException()
        {
            var filter = FilterFactory.CreateEmpty<Blend>();

            Assert.Throws<InvalidOperationException>(() => filter.ToString());
        }

        [Fact]
        public void ColorBalance_EmptyToString_ThrowsException()
        {
            var filter = FilterFactory.CreateEmpty<ColorBalance>();

            Assert.Throws<InvalidOperationException>(() => filter.ToString());
        }

        [Fact]
        public void Concat_EmptyToString_ThrowsException()
        {
            var filter = FilterFactory.CreateEmpty<Concat>();

            Assert.Throws<InvalidOperationException>(() => filter.ToString());
        }

        [Fact]
        public void Fade_EmptyToString_ThrowsException()
        {
            var filter = FilterFactory.CreateEmpty<Fade>();

            Assert.Throws<InvalidOperationException>(() => filter.ToString());
        }

        [Fact]
        public void Movie_EmptyToString_ThrowsException()
        {
            var filter = FilterFactory.CreateEmpty<Movie>();

            Assert.Throws<InvalidOperationException>(() => filter.ToString());
        }

        [Fact]
        public void Scale_EmptyToString_ThrowsException()
        {
            var filter = FilterFactory.CreateEmpty<Scale>();

            Assert.Throws<InvalidOperationException>(() => filter.ToString());
        }

        [Fact]
        public void SetDar_EmptyToString_ThrowsException()
        {
            var filter = FilterFactory.CreateEmpty<SetDar>();

            Assert.Throws<InvalidOperationException>(() => filter.ToString());
        }

        [Fact]
        public void SetSar_EmptyToString_ThrowsException()
        {
            var filter = FilterFactory.CreateEmpty<SetSar>();

            Assert.Throws<InvalidOperationException>(() => filter.ToString());
        }

        [Fact]
        public void Volume_EmptyToString_ThrowsException()
        {
            var filter = FilterFactory.CreateEmpty<Volume>();

            Assert.Throws<InvalidOperationException>(() => filter.ToString());
        }
        #endregion

        #region Applies To
        [Fact]
        public void AFade_AppliesTo()
        {
            TestAppliesTo<AFade>(false, true, false);
        }

        [Fact]
        public void AAMix_AppliesTo()
        {
            TestAppliesTo<AMix>(false, true, false);
        }

        [Fact]
        public void AMovie_AppliesTo_Audio()
        {
            TestAppliesTo<AMovie>(false, true, false);
        }

        [Fact]
        public void Blend_AppliesTo_Audio()
        {
            TestAppliesTo<Blend>(true, false, false);
        }

        [Fact]
        public void ColorBalance_AppliesTo_Audio()
        {
            TestAppliesTo<ColorBalance>(true, false, false);
        }

        [Fact]
        public void Concat_AppliesTo_Audio()
        {
            TestAppliesTo<Concat>(true, true, false);
        }

        [Fact]
        public void Fade_AppliesTo_Audio()
        {
            TestAppliesTo<Fade>(true, false, false);
        }

        [Fact]
        public void Movie_AppliesTo_Audio()
        {
            TestAppliesTo<Movie>(true, false, true);
        }

        [Fact]
        public void Overlay_AppliesTo_Audio()
        {
            TestAppliesTo<Overlay>(true, false, true);
        }

        [Fact]
        public void Scale_AppliesTo_Audio()
        {
            TestAppliesTo<Scale>(true, false, false);
        }

        [Fact]
        public void SetDar_AppliesTo_Audio()
        {
            TestAppliesTo<SetDar>(true, false, true);
        }

        [Fact]
        public void SetSar_AppliesTo_Audio()
        {
            TestAppliesTo<SetSar>(true, false, true);
        }

        [Fact]
        public void Volume_AppliesTo_Audio()
        {
            TestAppliesTo<Volume>(false, true, false);
        }


        private void TestAppliesTo<TFilter>(bool iVideo, bool iAudio, bool iImage)
            where TFilter : IFilter
        {
            Assert.True(iVideo == Validate.AppliesTo<TFilter, IVideo>());
            Assert.True(iAudio == Validate.AppliesTo<TFilter, IAudio>());
            Assert.True(iImage == Validate.AppliesTo<TFilter, IImage>());
        }
        #endregion

        #region Filterchain Tests
        //[Fact]
        //public void Crossfade_Setup_2Videos()
        //{
        //    var command = CommandBuilder.CreateNewWithXVideos(2);
        //    var receiptList = new List<CommandReceipt>()
        //        {
        //            command.ResourceReceiptAt(0), 
        //            command.ResourceReceiptAt(1)
        //        };

        //    Assert.DoesNotThrow(() => PerformCrossfade(command, receiptList));

        //    Assert.Equal(command.Filtergraph.Count, 7);
        //}

        //[Fact]
        //public void Crossfade_Setup_3Videos()
        //{
        //    var command = CommandBuilder.CreateNewWithXVideos(3);
        //    var receiptList = new List<CommandReceipt>()
        //        {
        //            command.ResourceReceiptAt(0), 
        //            command.ResourceReceiptAt(1),
        //            command.ResourceReceiptAt(2)
        //        };

        //    Assert.DoesNotThrow(() => PerformCrossfade(command, receiptList));

        //    Assert.Equal(command.Filtergraph.Count, 13);
        //}

        //[Fact]
        //public void Crossfade_Setup_4Videos()
        //{
        //    var command = CommandBuilder.CreateNewWithXVideos(4);
        //    var receiptList = new List<CommandReceipt>()
        //        {
        //            command.ResourceReceiptAt(0), 
        //            command.ResourceReceiptAt(1),
        //            command.ResourceReceiptAt(2),
        //            command.ResourceReceiptAt(3)
        //        };

        //    Assert.DoesNotThrow(() => PerformCrossfade(command, receiptList));
        
        //    Assert.Equal(command.Filtergraph.Count, 19);
        //}

        //private void PerformCrossfade(FfmpegCommand command, List<CommandReceipt> receiptList)
        //{
        //    var filterchain = Filterchain.FilterTo<Mp4>(
        //                new Crossfade(TimeSpan.FromSeconds(1), Resolution1080P.Create<Mp4>())
        //            );

        //    CommandReceipt lastReceipt = null;
        //    receiptList.ForEach(r =>
        //    {
        //        if (lastReceipt == null)
        //        {
        //            lastReceipt = r;
        //            return;
        //        }

        //        command.WithStreams(lastReceipt, r)
        //               .Filter(filterchain);
        //        lastReceipt = r;
        //    });
        //}
        #endregion 

        private class FilterFactory
        {
            internal static TFilter CreateEmpty<TFilter>()
                where TFilter : IFilter, new()
            {
                return new TFilter();
            }
        }

        private class CommandBuilder
        {
            private static readonly CommandConfiguration TestConfiguration = new CommandConfiguration("C:/Source/Test", "C:/Source/Ffmpeg", "C:/Source/Assets"); 

            internal static FfmpegCommand CreateNewWithXVideos(int x)
            {
                var newFactory = new CommandFactory(TestConfiguration);
                var newCommand = newFactory.AsOutput();
                
                for (var i = 0; i < x; i++)
                {
                    newCommand.WithInput(Assets.Utilities.GetVideoFile()); 
                }

                return newCommand;
            }
        }
    }
}
