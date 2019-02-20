using System;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Settings;
using Xunit;

namespace Hudl.FFmpeg.Tests.Setting
{
    public class SettingCollectionTests
    {
        [Fact]
        public void SettingsCollection_ForInput_Valid()
        {
            var settingsCollection = SettingsCollection.ForInput(new SeekPositionInput(1));

            Assert.True(settingsCollection.Count == 1);
        }

        [Fact]
        public void SettingsCollection_ForInput_Invalid()
        {
            Assert.Throws<ArgumentException>(() => SettingsCollection.ForInput(new OverwriteOutput())); 
        }

        [Fact]
        public void SettingsCollection_ForOutput_Valid()
        {
            var settingsCollection = SettingsCollection.ForOutput(new OverwriteOutput());

            Assert.True(settingsCollection.Count == 1);
        }

        [Fact]
        public void SettingsCollection_ForOutput_Invalid()
        {
            Assert.Throws<ArgumentException>(() => SettingsCollection.ForOutput(new SeekPositionInput(1))); 
        }
        
        [Fact]
        public void SettingsCollection_ForAny()
        {
            SettingsCollection.ForOutput(new SampleRate(44100));
            SettingsCollection.ForInput(new SampleRate(44100));
        }

        [Fact]
        public void SettingsCollection_AllowMultiple()
        {
            var settingsCollectionO = SettingsCollection.ForOutput();

            settingsCollectionO.Add(new Map("test1"));

            settingsCollectionO.Add(new Map("test2"));

            Assert.True(settingsCollectionO.Count == 2);
        }

        [Fact]
        public void SettingsCollection_Add()
        {
            var settingsCollectionI = SettingsCollection.ForInput();
            var settingsCollectionO = SettingsCollection.ForOutput();
            
            Assert.Throws<ArgumentException>(() => settingsCollectionI.Add(new OverwriteOutput())); 

            Assert.Throws<ArgumentException>(() => settingsCollectionO.Add(new SeekPositionInput(1))); 

            settingsCollectionI.Add(new SeekPositionInput(1));

            settingsCollectionO.Add(new OverwriteOutput());

            Assert.True(settingsCollectionI.Count == 1);

            Assert.True(settingsCollectionO.Count == 1);
        }

        [Fact]
        public void SettingsCollection_AddRange()
        {
            var settingsCollectionI = SettingsCollection.ForInput();
            var settingsCollectionO = SettingsCollection.ForOutput();
            var settingsCollectionAddI = SettingsCollection.ForInput(new SeekPositionInput(1), new DurationInput(1));
            var settingsCollectionAddO = SettingsCollection.ForOutput(new OverwriteOutput(), new RemoveAudio());
            
            Assert.Throws<ArgumentNullException>(() => settingsCollectionI.AddRange(null)); 
            Assert.Throws<ArgumentNullException>(() => settingsCollectionO.AddRange(null)); 

            settingsCollectionI.AddRange(settingsCollectionAddI);
            settingsCollectionO.AddRange(settingsCollectionAddO);

            Assert.Throws<ArgumentException>(() => settingsCollectionI.AddRange(settingsCollectionAddO)); 
            Assert.Throws<ArgumentException>(() => settingsCollectionO.AddRange(settingsCollectionAddI)); 

            Assert.Throws<ArgumentException>(() => settingsCollectionI.AddRange(settingsCollectionAddI)); 
            Assert.Throws<ArgumentException>(() => settingsCollectionO.AddRange(settingsCollectionAddO)); 

            Assert.True(settingsCollectionI.Count == 2);
            Assert.True(settingsCollectionO.Count == 2);
        }

        [Fact]
        public void SettingsCollection_Merge()
        {
            var settingsCollectionI = SettingsCollection.ForInput(new SeekPositionInput(1));
            var settingsCollectionO = SettingsCollection.ForOutput(new CodecVideo(VideoCodecType.Libx264));
            var startAtDefault = new SeekPositionInput(2);
            var vcodecDefault = new CodecVideo(VideoCodecType.Copy);

            settingsCollectionI.Merge(startAtDefault, FFmpegMergeOptionType.OldWins);

            var startAtSetting = settingsCollectionI.Items[0] as SeekPositionInput; 
            Assert.False(startAtSetting != null && startAtSetting.Length == startAtDefault.Length);
            
            settingsCollectionI.Merge(startAtDefault, FFmpegMergeOptionType.NewWins);

            startAtSetting = settingsCollectionI.Items[0] as SeekPositionInput; 
            Assert.True(startAtSetting != null && startAtSetting.Length == startAtDefault.Length);

            Assert.Throws<ArgumentException>(() => settingsCollectionI.Merge(vcodecDefault, FFmpegMergeOptionType.OldWins));
            Assert.Throws<ArgumentException>(() => settingsCollectionI.Merge(vcodecDefault, FFmpegMergeOptionType.NewWins));


            settingsCollectionO.Merge(vcodecDefault, FFmpegMergeOptionType.OldWins);

            var vcodecSetting = settingsCollectionO.Items[0] as CodecVideo; 
            Assert.False(vcodecSetting != null && vcodecSetting.Codec == vcodecDefault.Codec);
            
            settingsCollectionO.Merge(vcodecDefault, FFmpegMergeOptionType.NewWins);

            vcodecSetting = settingsCollectionO.Items[0] as CodecVideo; 
            Assert.True(vcodecSetting != null && vcodecSetting.Codec == vcodecDefault.Codec);

            Assert.Throws<ArgumentException>(() => settingsCollectionO.Merge(startAtDefault, FFmpegMergeOptionType.OldWins));
            Assert.Throws<ArgumentException>(() => settingsCollectionO.Merge(startAtDefault, FFmpegMergeOptionType.NewWins));
        }

        [Fact]
        public void SettingsCollection_MergeRange()
        {
            var startAtDefault = new SeekPositionInput(2);
            var vcodecDefault = new CodecVideo(VideoCodecType.Copy);
            var settingsCollectionI = SettingsCollection.ForInput(new SeekPositionInput(1));
            var settingsCollectionO = SettingsCollection.ForOutput(new CodecVideo(VideoCodecType.Libx264));
            var settingsCollectionMergeI =  SettingsCollection.ForInput(startAtDefault);
            var settingsCollectionMergeO = SettingsCollection.ForOutput(vcodecDefault);

            
            Assert.Throws<ArgumentException>(() => settingsCollectionI.MergeRange(settingsCollectionMergeO, FFmpegMergeOptionType.OldWins));
            Assert.Throws<ArgumentException>(() => settingsCollectionO.MergeRange(settingsCollectionMergeI, FFmpegMergeOptionType.OldWins));
            Assert.Throws<ArgumentException>(() => settingsCollectionI.MergeRange(settingsCollectionMergeO, FFmpegMergeOptionType.NewWins));
            Assert.Throws<ArgumentException>(() => settingsCollectionO.MergeRange(settingsCollectionMergeI, FFmpegMergeOptionType.NewWins));

            settingsCollectionI.MergeRange(settingsCollectionMergeI, FFmpegMergeOptionType.OldWins);
            settingsCollectionO.MergeRange(settingsCollectionMergeO, FFmpegMergeOptionType.OldWins);

            var startAtSetting = settingsCollectionI.Items[0] as SeekPositionInput;
            var vcodecSetting = settingsCollectionO.Items[0] as CodecVideo;
            Assert.False(startAtSetting != null && startAtSetting.Length == startAtDefault.Length);
            Assert.False(vcodecSetting != null && vcodecSetting.Codec == vcodecDefault.Codec);
            
            settingsCollectionI.MergeRange(settingsCollectionMergeI, FFmpegMergeOptionType.NewWins);
            settingsCollectionO.MergeRange(settingsCollectionMergeO, FFmpegMergeOptionType.NewWins);

            startAtSetting = settingsCollectionI.Items[0] as SeekPositionInput;
            vcodecSetting = settingsCollectionO.Items[0] as CodecVideo;
            Assert.True(startAtSetting != null && startAtSetting.Length == startAtDefault.Length);
            Assert.True(vcodecSetting != null && vcodecSetting.Codec == vcodecDefault.Codec);
        }

        [Fact]
        public void SettingsCollection_Remove()
        {
            var settingsCollectionI = SettingsCollection.ForInput(new SeekPositionInput(1), new DurationInput(2));
            var settingsCollectionO = SettingsCollection.ForOutput(new RemoveAudio(), new OverwriteOutput());

            Assert.True(settingsCollectionI.Count == 2);
            Assert.True(settingsCollectionO.Count == 2);

            settingsCollectionI.Remove<SeekPositionInput>();
            settingsCollectionO.Remove<SeekPositionInput>();
            settingsCollectionI.Remove<RemoveAudio>();
            settingsCollectionO.Remove<RemoveAudio>();

            Assert.True(settingsCollectionI.Count == 1);
            Assert.True(settingsCollectionO.Count == 1);
        }

        [Fact]
        public void SettingsCollection_RemoveAt()
        {
            var settingsCollectionI = SettingsCollection.ForInput(new SeekPositionInput(1), new DurationInput(2));
            var settingsCollectionO = SettingsCollection.ForOutput(new RemoveAudio(), new OverwriteOutput());

            Assert.True(settingsCollectionI.Count == 2);
            Assert.True(settingsCollectionO.Count == 2);
            
            settingsCollectionI.RemoveAt(0);
            settingsCollectionO.RemoveAt(0);

            Assert.True(settingsCollectionI.Count == 1);
            Assert.True(settingsCollectionO.Count == 1);
        }

        [Fact]
        public void SettingsCollection_RemoveAll()
        {
            var settingsCollectionI = SettingsCollection.ForInput(new SeekPositionInput(1), new DurationInput(2));
            var settingsCollectionO = SettingsCollection.ForOutput(new RemoveAudio(), new OverwriteOutput());

            Assert.True(settingsCollectionI.Count == 2);
            Assert.True(settingsCollectionO.Count == 2);
            
            settingsCollectionI.RemoveAll(s => true);
            settingsCollectionO.RemoveAll(s => true);

            Assert.True(settingsCollectionI.Count == 0);
            Assert.True(settingsCollectionO.Count == 0);
        }
    }
}
