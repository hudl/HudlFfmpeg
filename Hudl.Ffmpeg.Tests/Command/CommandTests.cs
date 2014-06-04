using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;
using Hudl.Ffmpeg.Sugar;
using Hudl.Ffmpeg.Templates;
using Xunit;
using CommandFactory = Hudl.Ffmpeg.Command.CommandFactory;

namespace Hudl.Ffmpeg.Tests.Command
{
    public class CommandTests
    {
        private const string AssetPath = "c:/source/apples.mp4";
        private static readonly TimeSpan CommandLength = TimeSpan.FromSeconds(212);
        private static readonly SettingsCollection CommandSettingsI = SettingsCollection.ForInput(new StartAt(1));
        private static readonly SettingsCollection CommandSettingsI2 = SettingsCollection.ForInput(new Duration(1));
        private static readonly SettingsCollection CommandSettingsO = SettingsCollection.ForOutput(new OverwriteOutput());

        [Fact]
        public void Command_WithInput_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.Throws<ArgumentException>(() => command.WithInput(string.Empty));

            Assert.DoesNotThrow(() => command.WithInput(Assets.Utilities.GetVideoFile()));

            Assert.True(command.Resources.Count == 1);
        }

        [Fact]
        public void Command_WithOutput_Verify()
        {
            var command = CommandHelper.CreateCommandNoOut();

            Assert.Throws<ArgumentException>(() => command.WithOutput(string.Empty));

            Assert.Throws<ArgumentNullException>(() => command.WithOutput(AssetPath, null));

            Assert.Throws<ArgumentException>(() => command.WithOutput(AssetPath, SettingsCollection.ForInput()));

            Assert.DoesNotThrow(() => command.WithOutput(AssetPath, CommandSettingsO));

            Assert.True(command.Outputs.Count == 1);
        }

        [Fact]
        public void Command_ResourceReceiptAt_Verify()
        {
            var command = CommandHelper.CreateCommand();

            command.WithInput(Assets.Utilities.GetVideoFile());

            Assert.Throws<IndexOutOfRangeException>(() => command.ResourceReceiptAt(-1));

            Assert.Throws<IndexOutOfRangeException>(() => command.ResourceReceiptAt(1));

            Assert.DoesNotThrow(() => command.ResourceReceiptAt(0));
        }

        [Fact]
        public void Command_ResourceManager_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.Throws<ArgumentNullException>(() => command.ResourceManager.Add(null));

            Assert.Throws<ArgumentException>(() => command.ResourceManager.AddRange(null));

            Assert.Throws<ArgumentNullException>(() => command.ResourceManager.Insert(0, null));

            Assert.Throws<ArgumentNullException>(() => command.ResourceManager.Replace(null, null));

            Assert.DoesNotThrow(() => command.ResourceManager.Add(CommandResource.Create(Resource.VideoFrom(Assets.Utilities.GetVideoFile()))));

            var assetsList = new List<IResource>
                {
                    Resource.VideoFrom(Assets.Utilities.GetVideoFile()),
                    Resource.VideoFrom(Assets.Utilities.GetVideoFile())
                };
            var commandList = new List<CommandResource>
                {
                    CommandResource.Create(assetsList[0]),
                    CommandResource.Create(assetsList[1])
                }; 

            Assert.DoesNotThrow(() => command.ResourceManager.AddRange(commandList));

            Assert.DoesNotThrow(() => command.ResourceManager.Insert(0, CommandResource.Create(Resource.VideoFrom(Assets.Utilities.GetVideoFile()))));

            var receipt = command.ResourceManager.Add(CommandResource.Create(Resource.VideoFrom(Assets.Utilities.GetVideoFile())));
            var replaceWith = CommandResource.Create(Resource.VideoFrom(Assets.Utilities.GetVideoFile()));
            Assert.DoesNotThrow(() => command.ResourceManager.Replace(receipt, replaceWith));

            Assert.True(command.Resources.Count == 5);
        }

        [Fact]
        public void Command_OutputManager_Verify()
        {
            var command = CommandHelper.CreateCommandNoOut();

            Assert.Throws<ArgumentNullException>(() => command.OutputManager.Add(null));

            Assert.Throws<ArgumentException>(() => command.OutputManager.AddRange(null));

            Assert.DoesNotThrow(() => command.OutputManager.Add(CommandOutput.Create(Resource.VideoFrom(AssetPath))));

            var assetsList = new List<IResource>
                {
                    Resource.VideoFrom(AssetPath),
                    Resource.VideoFrom(AssetPath)
                };
            var commandList = new List<CommandOutput>
                {
                    CommandOutput.Create(assetsList[0]),
                    CommandOutput.Create(assetsList[1])
                };

            Assert.DoesNotThrow(() => command.OutputManager.AddRange(commandList));

            Assert.True(command.Outputs.Count == 3);
        }

        [Fact]
        public void Command_FilterchainManager_Verify()
        {
            var command = CommandHelper.CreateCommand();

            command.WithInput(Assets.Utilities.GetVideoFile())
                   .WithInput(Assets.Utilities.GetVideoFile())
                   .WithInput(Assets.Utilities.GetVideoFile())
                   .WithInput(Assets.Utilities.GetVideoFile());

            var receipt1 = command.ResourceReceiptAt(0);
            var receipt2 = command.ResourceReceiptAt(1);
            var receipt3 = command.ResourceReceiptAt(2);
            var receipt4 = command.ResourceReceiptAt(3);

            var filterchain = VideoCutTo.Create<Mp4>(1d, 2d);

            Assert.Throws<ArgumentNullException>(() => command.FilterchainManager.Add(null));

            Assert.Throws<ArgumentException>(() => command.FilterchainManager.Add(filterchain, null));
            
            Assert.Throws<ArgumentNullException>(() => command.FilterchainManager.AddToEach(null)); 
            
            Assert.Throws<ArgumentException>(() => command.FilterchainManager.AddToEach(filterchain, null));

            Assert.DoesNotThrow(() => command.FilterchainManager.Add(filterchain, receipt1));
            
            Assert.DoesNotThrow(() => command.FilterchainManager.AddToEach(filterchain, receipt2, receipt3, receipt4));

            Assert.True(command.Filtergraph.Count == 4);
        }

        [Fact]
        public void Command_RenderWith_Verify()
        {
            var command = CommandHelper.CreateCommand();

            command.WithInput(Assets.Utilities.GetVideoFile()); 

            Assert.DoesNotThrow(() => command.RenderWith<TestCommandProcessor>());
        }

        [Fact]
        public void Command_PreRenderAction_Verify()
        {
            var command = CommandHelper.CreateCommand();

            var beforeRenderExecuted = false;

            var stage = command.WithInput(Assets.Utilities.GetVideoFile())
                               .WithAllStreams()
                               .BeforeRender((c, f, b) =>
                                   {
                                       beforeRenderExecuted = true;
                                   });

            stage.Command.RenderWith<TestCommandProcessor>(); 

            Assert.True(beforeRenderExecuted);
        }

        [Fact]
        public void Command_PostRenderAction_Verify()
        {
            var command = CommandHelper.CreateCommand();

            var afterRenderExecuted = false;

            var stage = command.WithInput(Assets.Utilities.GetVideoFile())
                               .WithAllStreams()
                               .AfterRender((c, f, b) =>
                                   {
                                       afterRenderExecuted = true;
                                   });

            stage.Command.RenderWith<TestCommandProcessor>();

            Assert.True(afterRenderExecuted);
        }

        private class CommandHelper
        {
            private const string OutputVideo = "c:/source/output.mp4";
            private static readonly CommandConfiguration _testConfiguration = new CommandConfiguration("C:/Source/Test", "C:/Source/Ffmpeg", "C:/Source/Assets");

            public static FfmpegCommand CreateCommand()
            {
                var factory = new CommandFactory(_testConfiguration);

                return factory.AsOutput()
                              .WithOutput(OutputVideo); 
            }

            public static FfmpegCommand CreateCommandNoOut()
            {
                var factory = new CommandFactory(_testConfiguration);

                return factory.AsOutput();
            }

        }
    }
}
