using System;
using Hudl.FFmpeg.DataTypes;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.Serialization;
using Xunit;

namespace Hudl.FFmpeg.Tests.Setting
{
    public class SettingTests
    {

        [Fact]
        public void CopyTimestamps_Verify()
        {
            var setting = new CopyTimestamps();

            Assert.Equal("-copyts", SettingSerializer.Serialize(setting));
        }


        [Fact]
        public void FrameDropThreshold_Verify()
        {
            var setting1 = new FrameDropThreshold() { Threshold = 0 };
            var setting2 = new FrameDropThreshold(0);

            Assert.Equal("-frame_drop_threshold 0", SettingSerializer.Serialize(setting1));
            Assert.Equal("-frame_drop_threshold 0", SettingSerializer.Serialize(setting2));
        }

        [Fact]
        public void QualityScaleVideo_Verify()
        {
            var setting = new QualityScaleVideo(1);

            Assert.Throws<ArgumentException>(() => new QualityScaleAudio(0));
            Assert.Throws<ArgumentException>(() => new QualityScaleAudio(32));
            Assert.Equal("-q:v 1", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void QualityScaleAudio_Verify()
        {
            var setting = new QualityScaleAudio(1);

            Assert.Equal("-q:a 1", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void AspectRatio_Verify()
        {
            var setting = new AspectRatio(Ratio.Create(1, 1));

            Assert.Equal("-aspect 1:1", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void MovFlags_Verify()
        {
            var settingWrong1 = new MovFlags(string.Empty);
            var settingWrong2 = new MovFlags("  ");
            var setting = new MovFlags(MovFlags.EnableFastStart);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            Assert.Equal("-movflags +faststart", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void Map_Verify()
        {
            var settingWrong1 = new Map(string.Empty);
            var settingWrong2 = new Map("  ");
            var setting = new Map("output1");

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            Assert.Equal("-map [output1]", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void AutoConvert_Verify()
        {
            var setting = new AutoConvert();

            Assert.Equal("-auto_convert 1", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void BitrateAudio_Verify()
        {
            var settingWrong1 = new BitRateAudio(0);
            var setting = new BitRateAudio(1100);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Equal("-b:a 1100k", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void BitRateTolerance_Verify()
        {
            var settingWrong1 = new BitRateTolerance(0);
            var setting = new BitRateTolerance(1100);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Equal("-bt 1100k", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void BitRateVideo_Verify()
        {
            var settingWrong1 = new BitRateVideo(0);
            var setting = new BitRateVideo(1100);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Equal("-b:v 1100k", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void Channels_Verify()
        {
            var settingWrong1 = new ChannelOutput(0);
            var settingWrong2 = new ChannelOutput(-1);
            var setting = new ChannelOutput(1);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            Assert.Equal("-ac 1", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void CodecAudio_Verify()
        {
            var settingWrong1 = new CodecAudio(string.Empty);
            var settingWrong2 = new CodecAudio("  ");
            var setting = new CodecAudio(AudioCodecType.LibFdk_Aac);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            Assert.Equal("-c:a libfdk_aac", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void CodecVideo_Verify()
        {
            var settingWrong1 = new CodecVideo(string.Empty);
            var settingWrong2 = new CodecVideo("  ");
            var setting = new CodecVideo(VideoCodecType.Libx264);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            Assert.Equal("-c:v libx264", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void ConstantRateFactor_Verify()
        {
            var settingWrong1 = new ConstantRateFactor(-1);
            var settingWrong2 = new ConstantRateFactor(60);
            var setting = new ConstantRateFactor(18);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            Assert.Equal("-crf 18", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void Size_Verify()
        {
            var setting = new Size(852, 480);

            Assert.Equal("-s 852x480", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void Duration_Verify()
        {
            var settingWrong1 = new DurationInput(0);
            var setting = new DurationInput(2);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Equal("-t 00:00:02.0", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void FormatInput_Verify()
        {
            var settingWrong1 = new FormatInput(string.Empty);
            var settingWrong2 = new FormatInput("  ");
            var setting = new FormatInput("mp4");

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            Assert.Equal("-f mp4", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void FormatOutput_Verify()
        {
            var settingWrong1 = new FormatOutput(string.Empty);
            var settingWrong2 = new FormatOutput("  ");
            var setting = new FormatOutput("mp4");

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            Assert.Equal("-f mp4", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void FrameRate_Verify()
        {
            var settingWrong1 = new FrameRate();
            var setting = new FrameRate(29.97);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Equal("-r 29.97", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void Input_Verify()
        {
            var setting = new Input(Resource.From("c:\\apple.mp4"));

            Assert.Equal("-i \"c:/apple.mp4\"", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void Level_Verify()
        {
            var settingWrong1 = new Level(-1);
            var setting = new Level(3.1);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Equal("-level 3.1", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void OverwriteOutput_Verify()
        {
            var setting = new OverwriteOutput();

            Assert.Equal("-y", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void PixelFormat_Verify()
        {
            var settingWrong1 = new PixelFormat(string.Empty);
            var settingWrong2 = new PixelFormat("  ");
            var setting = new PixelFormat("yuv420p");

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            Assert.Equal("-pix_fmt yuv420p", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void ProfileVideo_Verify()
        {
            var settingWrong1 = new ProfileVideo(string.Empty);
            var settingWrong2 = new ProfileVideo("  ");
            var setting = new ProfileVideo("baseline");

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            Assert.Equal("-profile:v baseline", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void RemoveAudio_Verify()
        {
            var setting = new RemoveAudio();

            Assert.Equal("-an", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void AvoidNegativeTimestamps_Verify()
        {
            var setting1 = new AvoidNegativeTimestamps(AvoidNegativeTimestampsType.Auto);
            var setting2 = new AvoidNegativeTimestamps(AvoidNegativeTimestampsType.Disabled);
            var setting3 = new AvoidNegativeTimestamps(AvoidNegativeTimestampsType.MakeNonNegative);
            var setting4 = new AvoidNegativeTimestamps(AvoidNegativeTimestampsType.MakeZero);

            Assert.Equal("-avoid_negative_ts auto", SettingSerializer.Serialize(setting1));
            Assert.Equal("-avoid_negative_ts disabled", SettingSerializer.Serialize(setting2));
            Assert.Equal("-avoid_negative_ts make_non_negative", SettingSerializer.Serialize(setting3));
            Assert.Equal("-avoid_negative_ts make_zero", SettingSerializer.Serialize(setting4));
        }

        [Fact]
        public void SeekPositionOutput_Verify()
        {
            var settingWrong1 = new SeekPositionOutput(0);
            var settingWrong2 = new SeekPositionOutput(-1);
            var setting = new SeekPositionOutput(120);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            Assert.Equal("-ss 00:02:00.0", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void SeekPositionInput_Verify()
        {
            var settingWrong1 = new SeekPositionInput(0);
            var settingWrong2 = new SeekPositionInput(-1);
            var setting = new SeekPositionInput(120);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            Assert.Equal("-ss 00:02:00.0", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void TrimShortest_Verify()
        {
            var setting = new TrimShortest();

            Assert.Equal("-shortest", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void BitStreamFilterAudio_Verify()
        {
            var settingWrong1 = new BitStreamFilterAudio("");
            var setting = new BitStreamFilterAudio("aac_adtstoasc");

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Equal("-bsf:a aac_adtstoasc", SettingSerializer.Serialize(setting));
        }

        [Fact]
        public void BitStreamFilterVideo_Verify()
        {
            var settingWrong1 = new BitStreamFilterVideo("");
            var setting = new BitStreamFilterVideo("aac_adtstoasc");

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Equal("-bsf:v aac_adtstoasc", SettingSerializer.Serialize(setting));
        }
    }
}
