using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Resources.BaseTypes;
using Hudl.Ffmpeg.Settings;
using Hudl.Ffmpeg.Settings.BaseTypes;
using Hudl.Ffmpeg.Sugar;
using Xunit;
using CommandFactory = Hudl.Ffmpeg.Command.CommandFactory;

namespace Hudl.Ffmpeg.Tests.Command
{
    public class CommandTests
    {
        private const string AssetPath = "c:/source/apples.mp4";
        private static readonly SettingsCollection CommandSettingsO = SettingsCollection.ForOutput(new OverwriteOutput());

        public CommandTests()
        {
            Assets.Utilities.SetGlobalAssets();
        }

        [Fact]
        public void Command_WithInput_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.Throws<ArgumentException>(() => command.WithInput(string.Empty));

            Assert.DoesNotThrow(() => command.WithInput(Assets.Utilities.GetVideoFile()));

            Assert.True(command.Inputs.Count == 1);
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

            Assert.Throws<ArgumentNullException>(() => command.InputManager.Add(null));

            Assert.Throws<ArgumentException>(() => command.InputManager.AddRange(null));

            Assert.Throws<ArgumentNullException>(() => command.InputManager.Insert(0, null));

            Assert.Throws<ArgumentNullException>(() => command.InputManager.Replace(null, null));

            Assert.DoesNotThrow(() => command.InputManager.Add(CommandInput.Create(Resource.VideoFrom(Assets.Utilities.GetVideoFile()))));

            var assetsList = new List<IResource>
                {
                    Resource.VideoFrom(Assets.Utilities.GetVideoFile()),
                    Resource.VideoFrom(Assets.Utilities.GetVideoFile())
                };
            var commandList = new List<CommandInput>
                {
                    CommandInput.Create(assetsList[0]),
                    CommandInput.Create(assetsList[1])
                }; 

            Assert.DoesNotThrow(() => command.InputManager.AddRange(commandList));

            Assert.DoesNotThrow(() => command.InputManager.Insert(0, CommandInput.Create(Resource.VideoFrom(Assets.Utilities.GetVideoFile()))));

            var receipt = command.InputManager.Add(CommandInput.Create(Resource.VideoFrom(Assets.Utilities.GetVideoFile())));
            var replaceWith = CommandInput.Create(Resource.VideoFrom(Assets.Utilities.GetVideoFile()));
            Assert.DoesNotThrow(() => command.InputManager.Replace(receipt, replaceWith));

            Assert.True(command.Inputs.Count == 5);
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

            var filterchain = Filterchain.FilterTo<Mp4>(new Trim(1, 2, VideoUnitType.Seconds));

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
                               .AfterRender((c, f, b) =>
                                   {
                                       afterRenderExecuted = true;
                                   });

            stage.Command.RenderWith<TestCommandProcessor>();

            Assert.True(afterRenderExecuted);
        }

        [Fact]
        public void Command_WithInputVsAddInput_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.Throws<ArgumentException>(() => command.WithInput(string.Empty));
            Assert.Throws<ArgumentException>(() => command.AddInput(string.Empty));

            var stage = new CommandStage(command);

            Assert.DoesNotThrow(() =>
                {
                    stage = command.AddInput(Assets.Utilities.GetVideoFile())
                                   .WithInput(Assets.Utilities.GetVideoFile());
                });

            Assert.True(command.Inputs.Count == 2);
            Assert.True(stage.Receipts.Count == 1);
        }

        private static class CommandHelper
        {
            private const string OutputVideo = "c:/source/output.mp4";

            public static FfmpegCommand CreateCommand()
            {
                var factory = CommandFactory.Create();

                return factory.CreateOutputCommand()
                              .WithOutput(OutputVideo); 
            }

            public static FfmpegCommand CreateCommandNoOut()
            {
                var factory = CommandFactory.Create();

                return factory.CreateOutputCommand();
            }

        }
    }
}
