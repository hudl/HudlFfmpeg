using System.Drawing;
using System.Text;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Common.DataTypes;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Filters;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Sugar;
using Hudl.FFmpeg.Tests.Filter.Identify;
using Xunit;

namespace Hudl.FFmpeg.Tests.Filter
{
    public class FilterTests
    {
        #region Applies To
        [Fact]
        public void AFade_AppliesTo()
        {
            TestAppliesTo<AFade>(false, true);
        }

        [Fact]
        public void AAMix_AppliesTo()
        {
            TestAppliesTo<AMix>(false, true);
        }

        [Fact]
        public void AMovie_AppliesTo_Audio()
        {
            TestAppliesTo<AMovie>(false, true);
        }

        [Fact]
        public void Blend_AppliesTo_Audio()
        {
            TestAppliesTo<Blend>(true, false);
        }

        [Fact]
        public void ColorBalance_AppliesTo_Audio()
        {
            TestAppliesTo<ColorBalance>(true, false);
        }

        [Fact]
        public void Concat_AppliesTo_Audio()
        {
            TestAppliesTo<Concat>(true, true);
        }

        [Fact]
        public void Fade_AppliesTo_Audio()
        {
            TestAppliesTo<Fade>(true, false);
        }

        [Fact]
        public void Movie_AppliesTo_Audio()
        {
            TestAppliesTo<Movie>(true, false);
        }

        [Fact]
        public void Overlay_AppliesTo_Audio()
        {
            TestAppliesTo<Overlay>(true, false);
        }

        [Fact]
        public void Scale_AppliesTo_Audio()
        {
            TestAppliesTo<Scale>(true, false);
        }

        [Fact]
        public void SetDar_AppliesTo_Audio()
        {
            TestAppliesTo<SetDar>(true, false);
        }

        [Fact]
        public void SetSar_AppliesTo_Audio()
        {
            TestAppliesTo<SetSar>(true, false);
        }

        [Fact]
        public void Volume_AppliesTo_Audio()
        {
            TestAppliesTo<Volume>(false, true);
        }


        private void TestAppliesTo<TFilter>(bool iVideo, bool iAudio)
            where TFilter : IFilter
        {
            Assert.True(iVideo == Validate.AppliesTo<TFilter, VideoStream>());
            Assert.True(iAudio == Validate.AppliesTo<TFilter, AudioStream>());
        }
        #endregion

        [Fact]
        public void AFade_Verify()
        {
            var filter = FilterFactory.CreateEmpty<AFade>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.TransitionType = FadeTransitionType.Out;
            filterValue.Append("afade=t=out");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.StartSample = 1D;
            filterValue.Append(":ss=1");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.NumberOfSamples = 2D;
            filterValue.Append(":ns=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.StartTime = 2D;
            filterValue.Append(":st=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Duration = 2D;
            filterValue.Append(":d=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void Fade_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Fade>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.TransitionType = FadeTransitionType.Out;
            filterValue.Append("fade=t=out");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.StartFrame = 1D;
            filterValue.Append(":s=1");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.NumberOfFrames = 2D;
            filterValue.Append(":n=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.StartTime = 2D;
            filterValue.Append(":st=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Duration = 2D;
            filterValue.Append(":d=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Alpha = true;
            filterValue.Append(":alpha=1");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Color = "blue";
            filterValue.Append(":c=blue");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void BoxBlur_Verify()
        {
            var filter = FilterFactory.CreateEmpty<BoxBlur>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.Expression = "15:1"; 
            filterValue.Append("boxblur=15:1");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void AMix_Verify()
        {
            var filter = FilterFactory.CreateEmpty<AMix>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.Inputs = 3;
            filterValue.Append("amix=inputs=3");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Duration = DurationType.First;
            filterValue.Append(":duration=first");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.DropoutTransition = 2D;
            filterValue.Append(":dropout_transition=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void AMovie_Verify()
        {
            var filter = FilterFactory.CreateEmpty<AMovie>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.Resource = Resource.From(Assets.Utilities.GetAudioFile());
            filterValue.AppendFormat("amovie=filename={0}", filter.Resource.Path);
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.FormatName = "mp4";
            filterValue.Append(":f=mp4");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.SeekPoint = 2D;
            filterValue.Append(":sp=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Streams = "qa";
            filterValue.Append(":s=qa");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.StreamIndex = 1;
            filterValue.Append(":si=1");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Loop = 2;
            filterValue.Append(":loop=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void Movie_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Movie>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.Resource = Resource.From(Assets.Utilities.GetAudioFile());
            filterValue.AppendFormat("movie=filename={0}", filter.Resource.Path);
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.FormatName = "mp4";
            filterValue.Append(":f=mp4");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.SeekPoint = 2D;
            filterValue.Append(":sp=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Streams = "qa";
            filterValue.Append(":s=qa");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.StreamIndex = 1;
            filterValue.Append(":si=1");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Loop = 2;
            filterValue.Append(":loop=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void ASplit_Verify()
        {
            var filter = FilterFactory.CreateEmpty<ASplit>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.NumberOfStreams = 3;
            filterValue.Append("asplit=3");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void Split_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Split>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.NumberOfStreams = 3;
            filterValue.Append("split=3");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void Blend_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Blend>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.Option = BlendVideoOptionType.all_expr;
            filter.Expression = "A*(if(gte(T,{0}),1,T/{0}))+B*(1-(if(gte(T,{0}),1,T/{0})))";
            filterValue.Append("blend=all_expr='A*(if(gte(T,{0}),1,T/{0}))+B*(1-(if(gte(T,{0}),1,T/{0})))'");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void ColorBalance_Verify()
        {
            var filter = FilterFactory.CreateEmpty<ColorBalance>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.Shadow = new DecimalScaleRgb(
                new DecimalScale(.3m),
                new DecimalScale(.3m),
                new DecimalScale(.3m));
            filterValue.Append("colorbalance=rs=0.3:gs=0.3:bs=0.3");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Midtone = new DecimalScaleRgb(
                new DecimalScale(.3m),
                new DecimalScale(.3m),
                new DecimalScale(.3m));
            filterValue.Append(":rm=0.3:gm=0.3:bm=0.3");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Highlight = new DecimalScaleRgb(
                new DecimalScale(.3m),
                new DecimalScale(.3m),
                new DecimalScale(.3m));
            filterValue.Append(":rh=0.3:gh=0.3:bh=0.3");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void Concat_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Concat>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.NumberOfVideoOut = 2;
            filterValue.Append("concat=v=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.NumberOfAudioOut = 2;
            filterValue.Append(":a=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void Crop_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Crop>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.Dimensions = new System.Drawing.Size(2, 2);
            filterValue.Append("crop=w=2:h=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Offset = new Point(2, 2);
            filterValue.Append(":x=2:y=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void Fps_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Fps>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.FrameRate = 29.97;
            filterValue.Append("fps=29.97");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void Overlay_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Overlay>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.X = "1";
            filterValue.Append("overlay=x=1");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Y = "1";
            filterValue.Append(":y=1");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Eval = OverlayVideoEvalType.Init;
            filterValue.Append(":eval=init");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Format = OverlayVideoFormatType.Yuv444;
            filterValue.Append(":format=yuv444");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.EofAction = OverlayEofActionType.Pass;
            filterValue.Append(":eof_action=pass");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void Pad_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Pad>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.Dimensions = new System.Drawing.Size(2, 2);
            filterValue.Append("pad=w=2:h=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Offset = new Point(2, 2);
            filterValue.Append(":x=2:y=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Color = "blue";
            filterValue.Append(":color=blue");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void Scale_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Scale>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.Dimensions = new System.Drawing.Size(2, 2);
            filterValue.Append("scale=w=2:h=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Interlacing = 1; 
            filterValue.Append(":interl=1");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Flags = "lanczos";
            filterValue.Append(":flags=lanczos");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.InColorMatrix = VideoScalingColorMatrixType.Bt601;
            filterValue.Append(":in_color_matrix=bt601");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.OutColorMatrix = VideoScalingColorMatrixType.Bt601;
            filterValue.Append(":out_color_matrix=bt601");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.InRange = VideoScalingRangeType.Mpeg_Tv;
            filterValue.Append(":in_range=mpeg/tv");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.OutRange = VideoScalingRangeType.Mpeg_Tv;
            filterValue.Append(":out_range=mpeg/tv");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.ForceAspectRatio = VideoScalingAspectRatioType.Increase;
            filterValue.Append(":force_original_aspect_ratio=increase");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void SetDar_Verify()
        {
            var filter = FilterFactory.CreateEmpty<SetDar>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.Ratio = new Ratio(1, 1);
            filterValue.Append("setdar=dar=1/1");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void SetSar_Verify()
        {
            var filter = FilterFactory.CreateEmpty<SetSar>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.Ratio = new Ratio(1, 1);
            filterValue.Append("setsar=sar=1/1");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void SetPts_Verify()
        {
            var filter = FilterFactory.CreateEmpty<SetPts>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.Expression = SetPts.ResetPtsExpression;
            filterValue.Append("setpts=expr=PTS-STARTPTS");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void Trim_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Trim>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.Start = 2D; 
            filterValue.Append("trim=start=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.End = 2D;
            filterValue.Append(":end=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.StartPts = 2D;
            filterValue.Append(":start_pts=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.EndPts = 2D;
            filterValue.Append(":end_pts=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.StartFrame = 2D;
            filterValue.Append(":start_frame=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.EndFrame = 2D;
            filterValue.Append(":end_frame=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Duration = 2D;
            filterValue.Append(":duration=2");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void Volume_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Volume>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filter.Expression = "0.5";
            filterValue.Append("volume=volume=0.5");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Precision = VolumePrecisionType.Double;
            filterValue.Append(":precision=double");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.ReplayGain = VolumeReplayGainType.Ignore;
            filterValue.Append(":replaygain=ignore");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.ReplayGainPreamp = 2.2D; 
            filterValue.Append(":replaygain_preamp=2.2");
            Assert.Equal(filter.ToString(), filterValue.ToString());

            filter.Eval = VolumeExpressionEvalType.Frame;
            filterValue.Append(":eval=frame");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        [Fact]
        public void Custom_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Yadif>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => { var s = filter.ToString(); });

            filterValue.Append("yadif=1");
            Assert.Equal(filter.ToString(), filterValue.ToString());
        }

        private class FilterFactory
        {
            internal static TFilter CreateEmpty<TFilter>()
                where TFilter : IFilter, new()
            {
                return new TFilter();
            }
        }

    }
}
