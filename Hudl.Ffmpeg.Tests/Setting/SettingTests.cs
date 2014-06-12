using System;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Settings;
using Xunit;

namespace Hudl.Ffmpeg.Tests.Setting
{
    public class SettingTests
    {
        [Fact]
        public void AspectRatio_Verify()
        {
            var settingWrong1 = new AspectRatio();
            var setting = new AspectRatio(FfmpegRatio.Create(1, 1));

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-aspect 1:1");
        }

        [Fact]
        public void MovFlags_Verify()
        {
            var settingWrong1 = new MovFlags(string.Empty);
            var settingWrong2 = new MovFlags("  ");
            var setting = new MovFlags(MovFlags.EnableFastStart);

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong2.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-movflags +faststart");
        }

        [Fact]
        public void Map_Verify()
        {
            var settingWrong1 = new Map(string.Empty);
            var settingWrong2 = new Map("  ");
            var setting = new Map("output1");

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong2.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-map [output1]");
        }

        [Fact]
        public void AutoConvert_Verify()
        {
            var setting = new AutoConvert();

            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-auto_convert 1");
        }

        [Fact]
        public void BitrateAudio_Verify()
        {
            var settingWrong1 = new BitRateAudio(0);
            var setting = new BitRateAudio(1100);

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-b:a 1100k");
        }

        [Fact]
        public void BitRateTolerance_Verify()
        {
            var settingWrong1 = new BitRateTolerance(0);
            var setting = new BitRateTolerance(1100);

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-bt 1100k");
        }

        [Fact]
        public void BitRateVideo_Verify()
        {
            var settingWrong1 = new BitRateVideo(0);
            var setting = new BitRateVideo(1100);

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-b:v 1100k");
        }

        [Fact]
        public void Channels_Verify()
        {
            var settingWrong1 = new Channels(0);
            var settingWrong2 = new Channels(-1);
            var setting = new Channels(1);

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong2.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-ac 1");
        }

        [Fact]
        public void CodecAudio_Verify()
        {
            var settingWrong1 = new CodecAudio(string.Empty);
            var settingWrong2 = new CodecAudio("  ");
            var setting = new CodecAudio(AudioCodecType.LibFdk_Aac);

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong2.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-c:a libfdk_aac");
        }

        [Fact]
        public void CodecVideo_Verify()
        {
            var settingWrong1 = new CodecVideo(string.Empty);
            var settingWrong2 = new CodecVideo("  ");
            var setting = new CodecVideo(VideoCodecType.Libx264);

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong2.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-c:v libx264");
        }

        [Fact]
        public void ConstantRateFactor_Verify()
        {
            var settingWrong1 = new ConstantRateFactor(-1);
            var settingWrong2 = new ConstantRateFactor(60);
            var setting = new ConstantRateFactor(18);

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong2.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-crf 18");
        }

        [Fact]
        public void Size_Verify()
        {
            var settingWrong1 = new Size();
            var setting = new Size(852, 480);

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-s 852x480");
        }

        [Fact]
        public void Duration_Verify()
        {
            var settingWrong1 = new Duration(0);
            var setting = new Duration(2);

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-t 00:00:02");
        }

        [Fact]
        public void FormatInput_Verify()
        {
            var settingWrong1 = new FormatInput(string.Empty);
            var settingWrong2 = new FormatInput("  ");
            var setting = new FormatInput("mp4");

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong2.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-f mp4");
        }

        [Fact]
        public void FormatOutput_Verify()
        {
            var settingWrong1 = new FormatOutput(string.Empty);
            var settingWrong2 = new FormatOutput("  ");
            var setting = new FormatOutput("mp4");

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong2.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-f mp4");
        }

        [Fact]
        public void FrameRate_Verify()
        {
            var settingWrong1 = new FrameRate();
            var setting = new FrameRate(29.97);

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-r 29.97");
        }

        [Fact]
        public void Input_Verify()
        {
            var settingWrong1 = new Input(null);
            var setting = new Input(Resource.From("c:\\apple.mp4"));

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-i \"c:/apple.mp4\"");
        }

        [Fact]
        public void Level_Verify()
        {
            var settingWrong1 = new Level(-1);
            var setting = new Level(3.1);

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-level 3.1");
        }

        [Fact]
        public void OverwriteOutput_Verify()
        {
            var setting = new OverwriteOutput();

            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-y");
        }

        [Fact]
        public void PixelFormat_Verify()
        {
            var settingWrong1 = new PixelFormat(string.Empty);
            var settingWrong2 = new PixelFormat("  ");
            var setting = new PixelFormat("yuv420p");

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong2.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-pix_fmt yuv420p");
        }

        [Fact]
        public void ProfileVideo_Verify()
        {
            var settingWrong1 = new ProfileVideo(string.Empty);
            var settingWrong2 = new ProfileVideo("  ");
            var setting = new ProfileVideo("baseline");

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong2.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-profile:v baseline");
        }

        [Fact]
        public void RemoveAudio_Verify()
        {
            var setting = new RemoveAudio();

            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-an");
        }

        [Fact]
        public void SeekTo_Verify()
        {
            var settingWrong1 = new SeekTo(0);
            var settingWrong2 = new SeekTo(-1);
            var setting = new SeekTo(120);

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong2.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-ss 00:02:00");
        }

        [Fact]
        public void StartAt_Verify()
        {
            var settingWrong1 = new StartAt(0);
            var settingWrong2 = new StartAt(-1);
            var setting = new StartAt(120);

            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong1.Stringify(); });
            Assert.Throws<InvalidOperationException>(() => { var s = settingWrong2.Stringify(); });
            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-ss 00:02:00");
        }

        [Fact]
        public void TrimShortest_Verify()
        {
            var setting = new TrimShortest();

            Assert.DoesNotThrow(() => { var s = setting.Stringify(); });
            Assert.Equal(setting.Stringify(), "-shortest");
        }
    }
}
