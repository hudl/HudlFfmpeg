using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Command.Obsolete;
using Hudl.Ffmpeg.Filters.Obsolete;
using Hudl.Ffmpeg.Filters.Obsolete.BaseTypes;
using Hudl.Ffmpeg.Settings.Obsolete;
using Hudl.Ffmpeg.Settings.Obsolete.BaseTypes;
using Hudl.Ffmpeg.Tests.Command;
using Xunit;

namespace Hudl.Ffmpeg.Tests.Obsolete.Command
{
    public class CommandTests
    {
        private const string AssetPath = "apples.mp4";
        private const string CommandPath = "c:/source/apples.mp4";
        private static readonly TimeSpan CommandLength = TimeSpan.FromSeconds(212);
        private static readonly SettingsCollection CommandSettingsI = SettingsCollection.ForInput(new StartAt(1));
        private static readonly SettingsCollection CommandSettingsI2 = SettingsCollection.ForInput(new Duration(1));
        private static readonly SettingsCollection CommandSettingsO = SettingsCollection.ForOutput(new OverwriteOutput());

        [Fact]
        public void Command_AddResource_Path_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.Throws<ArgumentException>(() => command.AddResource<Mp4>(string.Empty));

            Assert.DoesNotThrow(() => command.AddResource<Mp4>(CommandPath));

            Assert.True(command.Resources.Count == 1);
        }

        [Fact]
        public void Command_AddAsset_Path_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.Throws<ArgumentException>(() => command.AddAsset<Mp4>(string.Empty));

            Assert.DoesNotThrow(() => command.AddAsset<Mp4>(AssetPath));

            Assert.True(command.Resources.Count == 1);
        }

        [Fact]
        public void Command_AddResource_PathAndLength_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.DoesNotThrow(() => command.AddResource<Mp4>(CommandPath, CommandLength));

            Assert.True(command.Resources.Count == 1);
        }

        [Fact]
        public void Command_AddAsset_PathAndLength_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.DoesNotThrow(() => command.AddAsset<Mp4>(AssetPath, CommandLength));

            Assert.True(command.Resources.Count == 1);
        }

        [Fact]
        public void Command_AddResource_SettingsAndPath_Verify()
        {
            var command = CommandHelper.CreateCommand();
            
            Assert.Throws<ArgumentNullException>(() => command.AddResource<Mp4>(null, CommandPath));

            Assert.Throws<ArgumentException>(() => command.AddResource<Mp4>(CommandSettingsO, CommandPath));

            Assert.DoesNotThrow(() => command.AddResource<Mp4>(CommandSettingsI, CommandPath));

            Assert.True(command.Resources.Count == 1);
        }

        [Fact]
        public void Command_AddAsset_SettingsAndPath_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.Throws<ArgumentNullException>(() => command.AddAsset<Mp4>(null, AssetPath));

            Assert.Throws<ArgumentException>(() => command.AddAsset<Mp4>(CommandSettingsO, AssetPath));

            Assert.DoesNotThrow(() => command.AddAsset<Mp4>(CommandSettingsI, AssetPath));

            Assert.True(command.Resources.Count == 1);
        }

        [Fact]
        public void Command_AddResource_SettingsAndPathAndLength_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.Throws<ArgumentNullException>(() => command.AddResource<Mp4>(null, CommandPath, CommandLength));

            Assert.Throws<ArgumentException>(() => command.AddResource<Mp4>(CommandSettingsO, CommandPath, CommandLength));

            Assert.DoesNotThrow(() => command.AddResource<Mp4>(CommandSettingsI, CommandPath, CommandLength));

            Assert.True(command.Resources.Count == 1);
        }

        [Fact]
        public void Command_AddAsset_SettingsAndPathAndLength_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.Throws<ArgumentNullException>(() => command.AddAsset<Mp4>(null, AssetPath, CommandLength));

            Assert.Throws<ArgumentException>(() => command.AddAsset<Mp4>(CommandSettingsO, AssetPath, CommandLength));

            Assert.DoesNotThrow(() => command.AddAsset<Mp4>(CommandSettingsI, AssetPath, CommandLength));

            Assert.True(command.Resources.Count == 1);
        }

        [Fact]
        public void Command_Add_Resource_Verify()
        {
            var command = CommandHelper.CreateCommand();
            var resource = CommandHelper.CreateResource();

            Assert.DoesNotThrow(() => command.Add(resource));

            Assert.Throws<ArgumentException>(() => command.Add(resource));

            Assert.True(command.Resources.Count == 1);
        }

        [Fact]
        public void Command_Add_SettingsAndResource_Verify()
        {
            var command = CommandHelper.CreateCommand();
            var resource = CommandHelper.CreateResource();

            Assert.DoesNotThrow(() => command.Add(CommandSettingsI, resource));

            Assert.Throws<ArgumentException>(() => command.Add(CommandSettingsO, resource));

            Assert.Throws<ArgumentException>(() => command.Add(CommandSettingsI, resource));

            Assert.True(command.Resources.Count == 1);
        }

        [Fact]
        public void Command_AddRange_ResourceList_Verify()
        {
            var command = CommandHelper.CreateCommand();
            var resourceList = new List<IResource>
                {
                    CommandHelper.CreateResource(),
                    CommandHelper.CreateResource()
                };

            Assert.DoesNotThrow(() => command.AddRange(resourceList));

            Assert.Throws<ArgumentException>(() => command.AddRange(resourceList));

            Assert.True(command.Resources.Count == 2);
        }

        [Fact]
        public void Command_AddRange_ResourceSettingsDictionary_Verify()
        {
            var command = CommandHelper.CreateCommand();
            var resourceDictionaryI = new Dictionary<SettingsCollection, IResource>
                {
                    { CommandSettingsI, CommandHelper.CreateResource()},
                    { CommandSettingsI2, CommandHelper.CreateResource()},
                };
            var resourceDictionaryO = new Dictionary<SettingsCollection, IResource>
                {
                    { CommandSettingsI, CommandHelper.CreateResource()},
                    { CommandSettingsO, CommandHelper.CreateResource()},
                };

            Assert.DoesNotThrow(() => command.AddRange(resourceDictionaryI));

            Assert.Throws<ArgumentException>(() => command.AddRange(resourceDictionaryO));

            Assert.Throws<ArgumentException>(() => command.AddRange(resourceDictionaryI));

            Assert.True(command.Resources.Count == 2);
        }

        [Fact]
        public void Command_GetReceipts_Verify()
        {
            var command = CommandHelper.CreateCommand();
            var receipt1 = command.AddResource<Mp4>(CommandPath);
            var receipt2 = command.AddResource<Mp4>(CommandPath);
            var receipts = command.GetAllReceipts();
            var receipts1 = command.GetAllReceipts(r => r.Map == receipt1.Map);
            var receipts2 = command.GetAllReceipts(r => r.Map == receipt2.Map);

            Assert.True(receipts.Count == 2);
            Assert.True(receipts1.Count == 1);
            Assert.True(receipts2.Count == 1);
        }

        [Fact]
        public void Command_ApplySettings_Setting_Verify()
        {
            var command = CommandHelper.CreateCommand();
            var startAtSetting = new StartAt(1);
            var durationSetting = new Duration(1);
            var receipt1 = command.AddResource<Mp4>(CommandPath);
            command.AddResource<Mp4>(CommandPath);

            Assert.DoesNotThrow(() => command.ApplySettings(startAtSetting));

            Assert.True(command.Resources[0].Settings.Contains<StartAt>());
            Assert.True(command.Resources[1].Settings.Contains<StartAt>());

            Assert.DoesNotThrow(() => command.ApplySettings(durationSetting, receipt1));
        }

        [Fact]
        public void Command_ApplySettings_SettingCollection_Verify()
        {
            var command = CommandHelper.CreateCommand();
            var receipt1 = command.AddResource<Mp4>(CommandPath);
            command.AddResource<Mp4>(CommandPath);

            Assert.DoesNotThrow(() => command.ApplySettings(CommandSettingsI));

            Assert.True(command.Resources[0].Settings.Contains<StartAt>());
            Assert.True(command.Resources[1].Settings.Contains<StartAt>());

            Assert.DoesNotThrow(() => command.ApplySettings(CommandSettingsI2, receipt1));

            Assert.True(command.Resources[0].Settings.Contains<Duration>());
        }

        [Fact]
        public void Command_ApplyFilter_Empty_Verify()
        {
            var command = CommandHelper.CreateCommand();
            command.AddResource<Mp4>(CommandPath);
            command.AddResource<Mp4>(CommandPath);

            Assert.DoesNotThrow(() => command.ApplyFilter<Mp4, ColorBalance>());

            Assert.True(command.Filterchains.Count == 1);
            Assert.True(command.Filterchains[0].Filters.Contains<ColorBalance>());
            Assert.True(command.Filterchains[0].Resources.Count == 2);
        }

        [Fact]
        public void Command_ApplyFilter_Filter_Verify()
        {
            var command = CommandHelper.CreateCommand();
            command.AddResource<Mp4>(CommandPath);
            command.AddResource<Mp4>(CommandPath);

            Assert.DoesNotThrow(() => command.ApplyFilter<Mp4, ColorBalance>(new ColorBalance()));

            Assert.True(command.Filterchains.Count == 1);
            Assert.True(command.Filterchains[0].Filters.Contains<ColorBalance>());
            Assert.True(command.Filterchains[0].Resources.Count == 2);
        }

        [Fact]
        public void Command_ApplyFilter_Filterchain_Verify()
        {
            var command = CommandHelper.CreateCommand();
            var filterchain1 = Filterchain.FilterTo<Mp4>(
                    new Overlay()
                );
            var filterchain2 = Filterchain.FilterTo<Mp4>(
                    new Fade()
                );

            var receipt1 = command.AddResource<Mp4>(CommandPath);
            command.AddResource<Mp4>(CommandPath);

            Assert.DoesNotThrow(() => command.ApplyFilter(filterchain2, receipt1));
            
            Assert.True(command.Filterchains.Count == 1);
            Assert.True(command.Filterchains[0].Filters.Contains<Fade>());
            Assert.True(command.Filterchains[0].Resources.Count == 1);

            Assert.DoesNotThrow(() => command.ApplyFilter(filterchain1));

            Assert.True(command.Filterchains.Count == 2);
            Assert.True(command.Filterchains[1].Filters.Contains<Overlay>());
            Assert.True(command.Filterchains[1].Resources.Count == 2);
        }

        [Fact]
        public void Command_ApplyFilterToEach_Empty_Verify()
        {
            var command = CommandHelper.CreateCommand();
            command.AddResource<Mp4>(CommandPath);
            command.AddResource<Mp4>(CommandPath);

            Assert.DoesNotThrow(() => command.ApplyFilterToEach<Mp4, ColorBalance>());

            Assert.True(command.Filterchains.Count == 2);
            Assert.True(command.Filterchains[0].Filters.Contains<ColorBalance>());
            Assert.True(command.Filterchains[0].Resources.Count == 1);
            Assert.True(command.Filterchains[1].Filters.Contains<ColorBalance>());
            Assert.True(command.Filterchains[1].Resources.Count == 1);
        }

        [Fact]
        public void Command_ApplyFilterToEach_Filter_Verify()
        {
            var command = CommandHelper.CreateCommand();
            command.AddResource<Mp4>(CommandPath);
            command.AddResource<Mp4>(CommandPath);

            Assert.DoesNotThrow(() => command.ApplyFilterToEach<Mp4, ColorBalance>(new ColorBalance()));

            Assert.True(command.Filterchains.Count == 2);
            Assert.True(command.Filterchains[0].Filters.Contains<ColorBalance>());
            Assert.True(command.Filterchains[0].Resources.Count == 1);
            Assert.True(command.Filterchains[1].Filters.Contains<ColorBalance>());
            Assert.True(command.Filterchains[1].Resources.Count == 1);
        }

        [Fact]
        public void Command_ApplyFilterToEach_Filterchain_Verify()
        {
            var command = CommandHelper.CreateCommand();
            var filterchain1 = Filterchain.FilterTo<Mp4>(
                    new Overlay()
                );
            var filterchain2 = Filterchain.FilterTo<Mp4>(
                    new Fade()
                );

            var receipt1 = command.AddResource<Mp4>(CommandPath);
            var receipt2 = command.AddResource<Mp4>(CommandPath);
            command.AddResource<Mp4>(CommandPath);

            Assert.DoesNotThrow(() => command.ApplyFilterToEach(filterchain2, receipt1, receipt2));

            Assert.True(command.Filterchains.Count == 2);
            Assert.True(command.Filterchains[0].Filters.Contains<Fade>());
            Assert.True(command.Filterchains[0].Resources.Count == 1);
            Assert.True(command.Filterchains[1].Filters.Contains<Fade>());
            Assert.True(command.Filterchains[1].Resources.Count == 1);

            Assert.DoesNotThrow(() => command.ApplyFilterToEach(filterchain1));

            Assert.True(command.Filterchains.Count == 5);
            Assert.True(command.Filterchains[2].Filters.Contains<Overlay>());
            Assert.True(command.Filterchains[2].Resources.Count == 1);
            Assert.True(command.Filterchains[3].Filters.Contains<Overlay>());
            Assert.True(command.Filterchains[3].Resources.Count == 1);
            Assert.True(command.Filterchains[4].Filters.Contains<Overlay>());
            Assert.True(command.Filterchains[4].Resources.Count == 1);
        }

        [Fact]
        public void Command_RenderWith_Verify()
        {
            var command = CommandHelper.CreateCommand();
            command.AddResource<Mp4>(CommandPath);
            Assert.DoesNotThrow(() => command.RenderWith<TestCommandProcessor>());
        }

        private class CommandHelper
        {
            private static readonly Ffmpeg.Command.CommandConfiguration TestConfiguration = new Ffmpeg.Command.CommandConfiguration("C:/Source/Test", "C:/Source/Ffmpeg", "C:/Source/Assets"); 

            public static Command<Mp4> CreateCommand()
            {
                var factory = new CommandFactory(TestConfiguration);
                return factory.CreateOutput<Mp4>();
            }

            public static Mp4 CreateResource()
            {
                return Resource.Create<Mp4>(CommandPath, CommandLength);
            }
        }
    }
}
