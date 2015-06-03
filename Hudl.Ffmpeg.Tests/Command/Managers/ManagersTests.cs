using System;
using System.Collections.Generic;
using System.Linq;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Filters;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Sugar;
using Xunit;

namespace Hudl.FFmpeg.Tests.Command.Managers
{
    public class ManagersTests
    {
        public ManagersTests()
        {
            Assets.Utilities.SetGlobalAssets();
        }
        
        [Fact]
        public void CommandFilterchainManager_Verify()
        {
            var factory = CommandFactory.Create();

            var command = factory.CreateOutputCommand()
                                 .WithInput<VideoStream>(Assets.Utilities.GetVideoFile())
                                 .WithInput<VideoStream>(Assets.Utilities.GetVideoFile());

            var commandFilterchainManager = FiltergraphManager.Create(command.Command);

            var filterchain = Filterchain.FilterTo<VideoStream>(new Fps());
            var filterchain2 = Filterchain.FilterTo<VideoStream>(new Concat());

            var streamIds = new List<StreamIdentifier>();

            Assert.DoesNotThrow(() => streamIds = commandFilterchainManager.AddToEach(filterchain, command.StreamIdentifiers.ToArray())); 
            Assert.True(streamIds.Count == 2);
            Assert.DoesNotThrow(() => streamIds = commandFilterchainManager.Add(filterchain2, streamIds.ToArray()));
            Assert.True(streamIds.Count == 1);
        }

        [Fact]
        public void CommandOutputManager_Verify()
        {
            var factory = CommandFactory.Create();

            var command = factory.CreateOutputCommand()
                                 .WithInput<VideoStream>(Assets.Utilities.GetVideoFile())
                                 .WithInput<VideoStream>(Assets.Utilities.GetVideoFile());

            var commandOutputManager = CommandOutputManager.Create(command.Command);

            var commandOutput = CommandOutput.Create(new Mp4()); 

            Assert.DoesNotThrow(() => commandOutputManager.Add(commandOutput));
            Assert.Throws<ArgumentException>(() => commandOutputManager.Add(commandOutput));
            Assert.True(command.Command.Outputs.Count == 1);
        }

        [Fact]
        public void CommandResource_Verify()
        {
            var factory = CommandFactory.Create();

            var command = factory.CreateOutputCommand();

            var resourceOne = CommandInput.Create(Resource.From(Assets.Utilities.GetVideoFile()));
            var resourceTwo = CommandInput.Create(Resource.From(Assets.Utilities.GetVideoFile()));
            var resourceList = new List<CommandInput>
                {
                    CommandInput.Create(Resource.From(Assets.Utilities.GetVideoFile())),
                    CommandInput.Create(Resource.From(Assets.Utilities.GetVideoFile()))
                }; 

            var commandResourceManager = CommandInputManager.Create(command);

            Assert.DoesNotThrow(() => commandResourceManager.Add(resourceOne));
            Assert.DoesNotThrow(() => commandResourceManager.AddRange(resourceList));
            Assert.DoesNotThrow(() => commandResourceManager.Insert(0, resourceTwo));
            Assert.True(command.Inputs.Count == 4);
        }
    }
}
