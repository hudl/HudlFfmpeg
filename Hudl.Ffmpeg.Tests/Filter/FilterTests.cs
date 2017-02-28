using System;
using System.Drawing;
using System.Text;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.DataTypes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Filters.Serialization;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Filters;
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

        [Fact]
        public void APad_AppliesTo_Audio()
        {
            TestAppliesTo<APad>(false, true);
        }

        private void TestAppliesTo<TFilter>(bool iVideo, bool iAudio)
            where TFilter : IFilter
        {
            Assert.True(iVideo == AttributeValidation.AttributeTypeEquals<TFilter, VideoStream>());
            Assert.True(iAudio == AttributeValidation.AttributeTypeEquals<TFilter, AudioStream>());
        }
        #endregion

        [Fact]
        public void APad_Verify()
        {
            var filter = FilterFactory.CreateEmpty<APad>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.PacketSize = 2;
            filterValue.Append("apad=packet_size=2");
            var sss = FilterSerializer.Serialize(filter);
            Assert.Equal(sss, filterValue.ToString());

            filter.PadLength = 2;
            filterValue.Append(":pad_len=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.WholeLength = 2;
            filterValue.Append(":whole_len=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void AFade_Verify()
        {
            var filter = FilterFactory.CreateEmpty<AFade>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.TransitionType = FadeTransitionType.Out;
            filterValue.Append("afade=t=out");
            var sss = FilterSerializer.Serialize(filter);
            Assert.Equal(sss, filterValue.ToString());

            filter.CurveType = FadeCurveType.Cub;
            filterValue.Append(":curve=cub");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.StartSample = 1D;
            filterValue.Append(":ss=1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.NumberOfSamples = 2D;
            filterValue.Append(":ns=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.StartTime = 2D;
            filterValue.Append(":st=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Duration = 2D;
            filterValue.Append(":d=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void Fade_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Fade>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.TransitionType = FadeTransitionType.Out;
            filterValue.Append("fade=t=out");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.StartFrame = 1D;
            filterValue.Append(":s=1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.NumberOfFrames = 2D;
            filterValue.Append(":n=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.StartTime = 2D;
            filterValue.Append(":st=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Duration = 2D;
            filterValue.Append(":d=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Alpha = true;
            filterValue.Append(":alpha=1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Color = "blue";
            filterValue.Append(":c=blue");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void BoxBlur_Verify()
        {
            var filter = FilterFactory.CreateEmpty<BoxBlur>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Expression = "15:1"; 
            filterValue.Append("boxblur=15:1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void AMix_Verify()
        {
            var filter = FilterFactory.CreateEmpty<AMix>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Inputs = 3;
            filterValue.Append("amix=inputs=3");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
       
            filter.DropoutTransition = 2D;
            filterValue.Append(":dropout_transition=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Duration = DurationType.First;
            filterValue.Append(":duration=first");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

        }

        [Fact]
        public void AMovie_Verify()
        {
            var filter = FilterFactory.CreateEmpty<AMovie>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Resource = Resource.From(Assets.Utilities.GetAudioFile());
            filterValue.AppendFormat("amovie=filename={0}", filter.Resource.FullName);
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.FormatName = "mp4";
            filterValue.Append(":f=mp4");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Streams = "qa";
            filterValue.Append(":s=qa");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.SeekPoint = 2D;
            filterValue.Append(":sp=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.StreamIndex = 1;
            filterValue.Append(":si=1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Loop = 2;
            filterValue.Append(":loop=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void Movie_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Movie>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Resource = Resource.From(Assets.Utilities.GetAudioFile());
            filterValue.AppendFormat("movie=filename={0}", filter.Resource.FullName);
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.FormatName = "mp4";
            filterValue.Append(":f=mp4");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Streams = "qa";
            filterValue.Append(":s=qa");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.SeekPoint = 2D;
            filterValue.Append(":sp=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
          
            filter.StreamIndex = 1;
            filterValue.Append(":si=1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Loop = 2;
            filterValue.Append(":loop=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void ASplit_Verify()
        {
            var filter = FilterFactory.CreateEmpty<ASplit>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.NumberOfStreams = 3;
            filterValue.Append("asplit=3");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void Split_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Split>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.NumberOfStreams = 3;
            filterValue.Append("split=3");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void Blend_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Blend>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Option = BlendVideoOptionType.all_expr;
            filter.Expression = "A*(if(gte(T,{0}),1,T/{0}))+B*(1-(if(gte(T,{0}),1,T/{0})))";
            filterValue.Append("blend=all_expr='A*(if(gte(T,{0}),1,T/{0}))+B*(1-(if(gte(T,{0}),1,T/{0})))'");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void ColorBalance_Verify()
        {
            var filter = FilterFactory.CreateEmpty<ColorBalance>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.ShadowRed = .3m;
            filter.ShadowGreen = .3m;
            filter.ShadowBlue = .3m;
            filterValue.Append("colorbalance=rs=0.3:gs=0.3:bs=0.3");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.MidtonesRed = .3m;
            filter.MidtonesGreen = .3m;
            filter.MidtonesBlue = .3m; 
            filterValue.Append(":rm=0.3:gm=0.3:bm=0.3");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.HighlightsRed = .3m;
            filter.HighlightsGreen = .3m;
            filter.HighlightsBlue = .3m; 
            filterValue.Append(":rh=0.3:gh=0.3:bh=0.3");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void Concat_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Concat>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.NumberOfVideoOut = 2;
            filterValue.Append("concat=v=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.NumberOfAudioOut = 2;
            filterValue.Append(":a=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.UnsafeMode = true;
            filterValue.Append(":unsafe");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void Crop_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Crop>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Width = 2; 
            filter.Height = 2; 
            filterValue.Append("crop=w=2:h=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.X = 2;
            filter.Y = 2; 
            filterValue.Append(":x=2:y=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void Fps_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Fps>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.FrameRate = 29.97;
            filterValue.Append("fps=fps=29.97");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void Overlay_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Overlay>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.X = "1";
            filterValue.Append("overlay=x=1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Y = "1";
            filterValue.Append(":y=1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Eval = OverlayVideoEvalType.Init;
            filterValue.Append(":eval=init");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
           
            filter.EofAction = OverlayEofActionType.Pass;
            filterValue.Append(":eof_action=pass");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Format = OverlayVideoFormatType.Yuv444;
            filterValue.Append(":format=yuv444");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void Pad_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Pad>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Width = 2; 
            filter.Height = 2; 
            filterValue.Append("pad=w=2:h=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.X = 2; 
            filter.Y = 2; 
            filterValue.Append(":x=2:y=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Color = "blue";
            filterValue.Append(":color=blue");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void Scale_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Scale>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Width = 2; 
            filter.Height = 2; 
            filterValue.Append("scale=w=2:h=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Interlacing = 1; 
            filterValue.Append(":interl=1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Flags = "lanczos";
            filterValue.Append(":flags=lanczos");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.InColorMatrix = VideoScalingColorMatrixType.Bt601;
            filterValue.Append(":in_color_matrix=bt601");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.OutColorMatrix = VideoScalingColorMatrixType.Bt601;
            filterValue.Append(":out_color_matrix=bt601");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.InRange = VideoScalingRangeType.Mpeg_Tv;
            filterValue.Append(":in_range=mpeg/tv");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.OutRange = VideoScalingRangeType.Mpeg_Tv;
            filterValue.Append(":out_range=mpeg/tv");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.ForceAspectRatio = VideoScalingAspectRatioType.Increase;
            filterValue.Append(":force_original_aspect_ratio=increase");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void SetDar_Verify()
        {
            var filter = FilterFactory.CreateEmpty<SetDar>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Ratio = new Ratio(1, 1);
            filterValue.Append("setdar=dar=1/1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void SetSar_Verify()
        {
            var filter = FilterFactory.CreateEmpty<SetSar>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Ratio = new Ratio(1, 1);
            filterValue.Append("setsar=sar=1/1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void SetPts_Verify()
        {
            var filter = FilterFactory.CreateEmpty<SetPts>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Expression = SetPts.ResetPtsExpression;
            filterValue.Append("setpts=expr=PTS-STARTPTS");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void Trim_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Trim>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Start = 2D; 
            filterValue.Append("trim=start=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.End = 2D;
            filterValue.Append(":end=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.StartPts = 2D;
            filterValue.Append(":start_pts=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.EndPts = 2D;
            filterValue.Append(":end_pts=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.StartFrame = 2D;
            filterValue.Append(":start_frame=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.EndFrame = 2D;
            filterValue.Append(":end_frame=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Duration = 2D;
            filterValue.Append(":duration=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void Volume_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Volume>();
            var filterValue = new StringBuilder(100);
                        Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Expression = "0.5";
            filterValue.Append("volume=volume=0.5");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.ReplayGainPreamp = 2.2D;
            filterValue.Append(":replaygain_preamp=2.2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Precision = VolumePrecisionType.Double;
            filterValue.Append(":precision=double");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.ReplayGain = VolumeReplayGainType.Ignore;
            filterValue.Append(":replaygain=ignore");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Eval = VolumeExpressionEvalType.Frame;
            filterValue.Append(":eval=frame");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void AEvalSrc_Verify()
        {
            var filter = FilterFactory.CreateEmpty<AEvalSrc>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Expression = "boom";
            filterValue.Append("aevalsrc=exprs=boom");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.ChannelLayout = "FL+CL";
            filterValue.Append(":c=FL+CL");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Duration = TimeSpan.FromSeconds(3.222);
            filterValue.Append(":d=00:00:03.222");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.SampleRate = 48000;
            filterValue.Append(":s=48000");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
            
            filter.NumberOfSamples = 2;
            filterValue.Append(":n=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void ANullSrc_Verify()
        {
            var filter = FilterFactory.CreateEmpty<ANullSrc>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.ChannelLayout = "FL+CL";
            filterValue.Append("anullsrc=cl=FL+CL");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.SampleRate = 48000;
            filterValue.Append(":r=48000");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.NumberOfSamples = 2;
            filterValue.Append(":n=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
        }

        [Fact]
        public void NullSrc_Verify()
        {
            var filter = FilterFactory.CreateEmpty<NullSrc>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Size = new Size(200, 200);
            filterValue.Append("nullsrc=s=200x200");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
       
            filter.FrameRate = 30;
            filterValue.Append(":r=30");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Duration = TimeSpan.FromSeconds(3.222);
            filterValue.Append(":d=00:00:03.222");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));
           
            filter.SampleAspectRatio = Ratio.Create(1, 1);
            filterValue.Append(":sar=1/1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

        }

        [Fact]
        public void Color_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Hudl.FFmpeg.Filters.Color>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.ColorName = "black";
            filterValue.Append("color=c=black");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Size = new Size(200, 200);
            filterValue.Append(":s=200x200");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.FrameRate = 30;
            filterValue.Append(":r=30");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Duration = TimeSpan.FromSeconds(3.222);
            filterValue.Append(":d=3.222");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.SampleAspectRatio = Ratio.Create(1, 1);
            filterValue.Append(":sar=1/1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

           
        }

        [Fact]
        public void ZoomPan_Verify()
        {
            var filter = FilterFactory.CreateEmpty<Hudl.FFmpeg.Filters.ZoomPan>();
            var filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter));

            filter.Zoom = "2";
            filterValue.Append("zoompan=zoom=2");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.X = "100";
            filterValue.Append(":x=100");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Y = "200";
            filterValue.Append(":y=200");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.D = "1";
            filterValue.Append(":d=1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.S = new Size(1280, 720);
            filterValue.Append(":s=1280x720");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            filter.Fps = 30;
            filterValue.Append(":fps=30");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter));

            var filter2 = FilterFactory.CreateEmpty<Hudl.FFmpeg.Filters.ZoomPan>();
            filterValue = new StringBuilder(100);
            Assert.DoesNotThrow(() => FilterSerializer.Serialize(filter2));

            filter2.Zoom = "in_w";
            filterValue.Append("zoompan=zoom='in_w'");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter2));

            filter2.X = "in_w+2";
            filterValue.Append(":x='in_w+2'");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter2));

            filter2.Y = "out_w+3";
            filterValue.Append(":y='out_w+3'");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter2));

            filter2.D = "1";
            filterValue.Append(":d=1");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter2));

            filter2.S = new Size(1280, 720);
            filterValue.Append(":s=1280x720");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter2));

            filter2.Fps = 30;
            filterValue.Append(":fps=30");
            Assert.Equal(filterValue.ToString(), FilterSerializer.Serialize(filter2));
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
