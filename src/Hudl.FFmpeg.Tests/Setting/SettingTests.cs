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

            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-copyts");
        }


        [Fact]
        public void FrameDropThreshold_Verify()
        {
            var setting1 = new FrameDropThreshold() { Threshold = 0 };
            var setting2 = new FrameDropThreshold(0);

            SettingSerializer.Serialize(setting1);
            SettingSerializer.Serialize(setting2);
            Assert.Equal(SettingSerializer.Serialize(setting1), "-frame_drop_threshold 0");
            Assert.Equal(SettingSerializer.Serialize(setting2), "-frame_drop_threshold 0");
        }

        [Fact]
        public void QualityScaleVideo_Verify()
        {
            var setting = new QualityScaleVideo(1);

            Assert.Throws<ArgumentException>(() => new QualityScaleAudio(0));
            Assert.Throws<ArgumentException>(() => new QualityScaleAudio(32));
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-q:v 1");
        }

        [Fact]
        public void QualityScaleAudio_Verify()
        {
            var setting = new QualityScaleAudio(1);

            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-q:a 1");
        }

        [Fact]
        public void AspectRatio_Verify()
        {
            var setting = new AspectRatio(Ratio.Create(1, 1));

            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-aspect 1:1");
        }

        [Fact]
        public void MovFlags_Verify()
        {
            var settingWrong1 = new MovFlags(string.Empty);
            var settingWrong2 = new MovFlags("  ");
            var setting = new MovFlags(MovFlags.EnableFastStart);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-movflags +faststart");
        }

        [Fact]
        public void Map_Verify()
        {
            var settingWrong1 = new Map(string.Empty);
            var settingWrong2 = new Map("  ");
            var setting = new Map("output1");

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-map [output1]");
        }

        [Fact]
        public void AutoConvert_Verify()
        {
            var setting = new AutoConvert();

            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-auto_convert 1");
        }

        [Fact]
        public void BitrateAudio_Verify()
        {
            var settingWrong1 = new BitRateAudio(0);
            var setting = new BitRateAudio(1100);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-b:a 1100k");
        }

        [Fact]
        public void BitRateTolerance_Verify()
        {
            var settingWrong1 = new BitRateTolerance(0);
            var setting = new BitRateTolerance(1100);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-bt 1100k");
        }

        [Fact]
        public void BitRateVideo_Verify()
        {
            var settingWrong1 = new BitRateVideo(0);
            var setting = new BitRateVideo(1100);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-b:v 1100k");
        }

        [Fact]
        public void Channels_Verify()
        {
            var settingWrong1 = new ChannelOutput(0);
            var settingWrong2 = new ChannelOutput(-1);
            var setting = new ChannelOutput(1);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-ac 1");
        }

        [Fact]
        public void CodecAudio_Verify()
        {
            var settingWrong1 = new CodecAudio(string.Empty);
            var settingWrong2 = new CodecAudio("  ");
            var setting = new CodecAudio(AudioCodecType.LibFdk_Aac);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-c:a libfdk_aac");
        }

        [Fact]
        public void CodecVideo_Verify()
        {
            var settingWrong1 = new CodecVideo(string.Empty);
            var settingWrong2 = new CodecVideo("  ");
            var setting = new CodecVideo(VideoCodecType.Libx264);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-c:v libx264");
        }

        [Fact]
        public void ConstantRateFactor_Verify()
        {
            var settingWrong1 = new ConstantRateFactor(-1);
            var settingWrong2 = new ConstantRateFactor(60);
            var setting = new ConstantRateFactor(18);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-crf 18");
        }

        [Fact]
        public void Size_Verify()
        {
            var setting = new Size(852, 480);

            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-s 852x480");
        }

        [Fact]
        public void Duration_Verify()
        {
            var settingWrong1 = new DurationInput(0);
            var setting = new DurationInput(2);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-t 00:00:02.0");
        }

        [Fact]
        public void FormatInput_Verify()
        {
            var settingWrong1 = new FormatInput(string.Empty);
            var settingWrong2 = new FormatInput("  ");
            var setting = new FormatInput("mp4");

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-f mp4");
        }

        [Fact]
        public void FormatOutput_Verify()
        {
            var settingWrong1 = new FormatOutput(string.Empty);
            var settingWrong2 = new FormatOutput("  ");
            var setting = new FormatOutput("mp4");

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-f mp4");
        }

        [Fact]
        public void FrameRate_Verify()
        {
            var settingWrong1 = new FrameRate();
            var setting = new FrameRate(29.97);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-r 29.97");
        }

        [Fact]
        public void Input_Verify()
        {
            var setting = new Input(Resource.From("c:\\apple.mp4"));

            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-i \"c:/apple.mp4\"");
        }

        [Fact]
        public void Level_Verify()
        {
            var settingWrong1 = new Level(-1);
            var setting = new Level(3.1);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-level 3.1");
        }

        [Fact]
        public void OverwriteOutput_Verify()
        {
            var setting = new OverwriteOutput();

            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-y");
        }

        [Fact]
        public void PixelFormat_Verify()
        {
            var settingWrong1 = new PixelFormat(string.Empty);
            var settingWrong2 = new PixelFormat("  ");
            var setting = new PixelFormat("yuv420p");

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-pix_fmt yuv420p");
        }

        [Fact]
        public void ProfileVideo_Verify()
        {
            var settingWrong1 = new ProfileVideo(string.Empty);
            var settingWrong2 = new ProfileVideo("  ");
            var setting = new ProfileVideo("baseline");

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-profile:v baseline");
        }

        [Fact]
        public void RemoveAudio_Verify()
        {
            var setting = new RemoveAudio();

            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-an");
        }

        [Fact]
        public void AvoidNegativeTimestamps_Verify()
        {
            var setting1 = new AvoidNegativeTimestamps(AvoidNegativeTimestampsType.Auto);
            var setting2 = new AvoidNegativeTimestamps(AvoidNegativeTimestampsType.Disabled);
            var setting3 = new AvoidNegativeTimestamps(AvoidNegativeTimestampsType.MakeNonNegative);
            var setting4 = new AvoidNegativeTimestamps(AvoidNegativeTimestampsType.MakeZero);

            SettingSerializer.Serialize(setting1);
            Assert.Equal(SettingSerializer.Serialize(setting1), "-avoid_negative_ts auto");

            SettingSerializer.Serialize(setting2);
            Assert.Equal(SettingSerializer.Serialize(setting2), "-avoid_negative_ts disabled");

            SettingSerializer.Serialize(setting3);
            Assert.Equal(SettingSerializer.Serialize(setting3), "-avoid_negative_ts make_non_negative");

            SettingSerializer.Serialize(setting4);
            Assert.Equal(SettingSerializer.Serialize(setting4), "-avoid_negative_ts make_zero");
        }

        [Fact]
        public void SeekPositionOutput_Verify()
        {
            var settingWrong1 = new SeekPositionOutput(0);
            var settingWrong2 = new SeekPositionOutput(-1);
            var setting = new SeekPositionOutput(120);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-ss 00:02:00.0");
        }

        [Fact]
        public void SeekPositionInput_Verify()
        {
            var settingWrong1 = new SeekPositionInput(0);
            var settingWrong2 = new SeekPositionInput(-1);
            var setting = new SeekPositionInput(120);

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong2); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-ss 00:02:00.0");
        }

        [Fact]
        public void TrimShortest_Verify()
        {
            var setting = new TrimShortest();

            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-shortest");
        }

        [Fact]
        public void BitStreamFilterAudio_Verify()
        {
            var settingWrong1 = new BitStreamFilterAudio("");
            var setting = new BitStreamFilterAudio("aac_adtstoasc");

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-bsf:a aac_adtstoasc");
        }

        [Fact]
        public void BitStreamFilterVideo_Verify()
        {
            var settingWrong1 = new BitStreamFilterVideo("");
            var setting = new BitStreamFilterVideo("aac_adtstoasc");

            Assert.Throws<InvalidOperationException>(() => { SettingSerializer.Serialize(settingWrong1); });
            SettingSerializer.Serialize(setting);
            Assert.Equal(SettingSerializer.Serialize(setting), "-bsf:v aac_adtstoasc");
        }
    }
}
