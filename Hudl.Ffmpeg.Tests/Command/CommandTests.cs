﻿using System;
using System.Collections.Generic;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Filters;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Sugar;
using Xunit;
using CommandFactory = Hudl.FFmpeg.Command.CommandFactory;

namespace Hudl.FFmpeg.Tests.Command
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
        public void Command_AddInput_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.Throws<ArgumentException>(() => command.AddInput(string.Empty));

            command.AddInput(Assets.Utilities.GetVideoFile());

            Assert.True(command.Inputs.Count == 1);
        }

        [Fact]
        public void Command_WithInput_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.Throws<ArgumentException>(() => command.WithInput<VideoStream>(string.Empty));

            command.WithInput<VideoStream>(Assets.Utilities.GetVideoFile());

            Assert.True(command.Inputs.Count == 1);
        }

        [Fact]
        public void Command_Select_Verify()
        {
            var command = CommandHelper.CreateCommand()
                                       .AddInput(Assets.Utilities.GetVideoFile())
                                       .AddInput(Assets.Utilities.GetVideoFile());

            Assert.Throws<IndexOutOfRangeException>(() => command.Select<VideoStream>(5));

            command.Select(1);

            command.Select(0);

            var stage = command.Select(0)
                               .Select(1);

            Assert.True(stage.StreamIdentifiers.Count == 2);
        }

        [Fact]
        public void Command_StreamIdentifier_Verify()
        {
            var command = CommandHelper.CreateCommand();

            command.AddInput(Assets.Utilities.GetVideoFile());

            Assert.Throws<IndexOutOfRangeException>(() => command.StreamIdentifier(-1));

            Assert.Throws<IndexOutOfRangeException>(() => command.StreamIdentifier(1));

            command.StreamIdentifier(0);
        }

        [Fact]
        public void Command_ResourceManager_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.Throws<ArgumentNullException>(() => command.InputManager.Add(null));

            Assert.Throws<ArgumentException>(() => command.InputManager.AddRange(null));

            Assert.Throws<ArgumentNullException>(() => command.InputManager.Insert(0, null));

            command.InputManager.Add(CommandInput.Create(Resource.From(Assets.Utilities.GetVideoFile())));

            var assetsList = new List<IContainer>
                {
                    Resource.From(Assets.Utilities.GetVideoFile()),
                    Resource.From(Assets.Utilities.GetVideoFile())
                };
            var commandList = new List<CommandInput>
                {
                    CommandInput.Create(assetsList[0]),
                    CommandInput.Create(assetsList[1])
                }; 

            command.InputManager.AddRange(commandList);

            command.InputManager.Insert(0, CommandInput.Create(Resource.From(Assets.Utilities.GetVideoFile())));

            Assert.True(command.Inputs.Count == 4);
        }

        [Fact]
        public void Command_OutputManager_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.Throws<ArgumentNullException>(() => command.OutputManager.Add(null));

            Assert.Throws<ArgumentException>(() => command.OutputManager.AddRange(null));

            command.OutputManager.Add(CommandOutput.Create(Resource.From(AssetPath)));

            var assetsList = new List<IContainer>
                {
                    Resource.From(AssetPath),
                    Resource.From(AssetPath)
                };
            var commandList = new List<CommandOutput>
                {
                    CommandOutput.Create(assetsList[0]),
                    CommandOutput.Create(assetsList[1])
                };

            command.OutputManager.AddRange(commandList);

            Assert.True(command.Outputs.Count == 3);
        }


        [Fact]
        public void Command_FilterWithNoStream()
        {
            var command = CommandHelper.CreateCommand();

            var filterchain = Filterchain.FilterTo<AudioStream>(new AEvalSrc {Expression = "0"});

            CommandStage stage = null;
            stage = command.Filter(filterchain);

            Assert.True(stage.StreamIdentifiers.Count == 1);
        }


        [Fact]
        public void Command_FilterchainManager_Verify()
        {
            var command = CommandHelper.CreateCommand();

            command.AddInput(Assets.Utilities.GetVideoFile())
                   .AddInput(Assets.Utilities.GetVideoFile())
                   .AddInput(Assets.Utilities.GetVideoFile())
                   .AddInput(Assets.Utilities.GetVideoFile());

            var streamId1 = command.StreamIdentifier(0);
            var streamId2 = command.StreamIdentifier(1);
            var streamId3 = command.StreamIdentifier(2);
            var streamId4 = command.StreamIdentifier(3);

            var filterchain = Filterchain.FilterTo<VideoStream>(new Trim(1, 2, VideoUnitType.Seconds));
            var filterchain2 = Filterchain.FilterTo<AudioStream>(new AEvalSrc());

            Assert.Throws<ArgumentNullException>(() => command.FilterchainManager.Add(null));

            Assert.Throws<InvalidOperationException>(() => command.FilterchainManager.Add(filterchain, null));
            
            Assert.Throws<ArgumentNullException>(() => command.FilterchainManager.AddToEach(null)); 
            
            Assert.Throws<ArgumentException>(() => command.FilterchainManager.AddToEach(filterchain, null));

            command.FilterchainManager.Add(filterchain2, null);

            command.FilterchainManager.Add(filterchain, streamId1);
            
            command.FilterchainManager.AddToEach(filterchain, streamId2, streamId3, streamId4);

            Assert.True(command.Filtergraph.Count == 5);
        }

        [Fact]
        public void Command_RenderWith_Verify()
        {
            var command = CommandHelper.CreateCommand();

            command.AddInput(Assets.Utilities.GetVideoFile());

            command.ExecuteWith<TestCommandProcessor, FFmpegCommandBuilder>(null);
        }

        [Fact]
        public void Command_PreRenderAction_Verify()
        {
            var command = CommandHelper.CreateCommand();

            var beforeRenderExecuted = false;

            var stage = command.WithInput<VideoStream>(Assets.Utilities.GetVideoFile())
                               .BeforeRender((c, f, b) =>
                                   {
                                       beforeRenderExecuted = true;
                                   });

            stage.Command.ExecuteWith<TestCommandProcessor, FFmpegCommandBuilder>(null); 

            Assert.True(beforeRenderExecuted);
        }

        [Fact]
        public void Command_PostRenderAction_Verify()
        {
            var command = CommandHelper.CreateCommand();

            var afterRenderExecuted = false;

            var stage = command.WithInput<VideoStream>(Assets.Utilities.GetVideoFile())
                               .AfterRender((c, f, b) =>
                                   {
                                       afterRenderExecuted = true;
                                   });

            stage.Command.ExecuteWith<TestCommandProcessor, FFmpegCommandBuilder>(null);

            Assert.True(afterRenderExecuted);
        }

        [Fact]
        public void Command_WithInputVsAddInput_Verify()
        {
            var command = CommandHelper.CreateCommand();

            Assert.Throws<ArgumentException>(() => command.AddInput(string.Empty));
            Assert.Throws<ArgumentException>(() => command.WithInput<VideoStream>(string.Empty));

            var stage = CommandStage.Create(command);

            stage = command.AddInput(Assets.Utilities.GetVideoFile())
                            .WithInput<VideoStream>(Assets.Utilities.GetVideoFile());

            Assert.True(command.Inputs.Count == 2);
            Assert.True(stage.StreamIdentifiers.Count == 1);
        }

        private static class CommandHelper
        {
            public static FFmpegCommand CreateCommand()
            {
                var factory = CommandFactory.Create();

                return factory.CreateOutputCommand();
            }

        }
    }
}
