using System;
using System.Collections.Generic;
using Hudl.Ffmpeg.Command;
using Hudl.Ffmpeg.Command.Managers;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Hudl.Ffmpeg.Sugar;
using Xunit;

namespace Hudl.Ffmpeg.Tests.Command.Managers
{
    public class ManagersTests
    {

        private readonly CommandConfiguration _testConfiguration = new CommandConfiguration("C:/Source/Test", "C:/Source/Ffmpeg", "C:/Source/Assets");

        [Fact]
        public void CommandFilterchainManager_Verify()
        {
            var factory = CommandFactory.Create(_testConfiguration);

            var command = factory.AsOutput()
                                 .WithInput(Assets.Utilities.GetVideoFile())
                                 .WithInput(Assets.Utilities.GetVideoFile())
                                 .WithAllStreams(); 

            var commandFilterchainManager = CommandFilterchainManager.Create(command.Command);

            var filterchain = Filterchain.FilterTo<Mp4>(new Fps());
            var filterchain2 = Filterchain.FilterTo<Mp4>(new Concat());

            var receipts = new List<CommandReceipt>();

            Assert.DoesNotThrow(() => receipts = commandFilterchainManager.AddToEach(filterchain, command.Receipts.ToArray())); 
            Assert.True(receipts.Count == 2);
            Assert.DoesNotThrow(() => receipts = commandFilterchainManager.Add(filterchain2, receipts.ToArray()));
            Assert.True(receipts.Count == 1);
        }

        [Fact]
        public void CommandOutputManager_Verify()
        {
            var factory = CommandFactory.Create(_testConfiguration);

            var command = factory.AsOutput()
                                 .WithInput(Assets.Utilities.GetVideoFile())
                                 .WithInput(Assets.Utilities.GetVideoFile())
                                 .WithAllStreams(); 

            var commandOutputManager = CommandOutputManager.Create(command.Command);

            var commandOutput = CommandOutput.Create(new Mp4()); 

            Assert.DoesNotThrow(() => commandOutputManager.Add(commandOutput));
            Assert.Throws<ArgumentException>(() => commandOutputManager.Add(commandOutput));
            Assert.True(command.Command.Outputs.Count == 1);
        }

        [Fact]
        public void CommandResource_Verify()
        {
            var factory = CommandFactory.Create(_testConfiguration);

            var command = factory.AsOutput();

            var resourceOne = CommandResource.Create(Resource.From(Assets.Utilities.GetVideoFile()));
            var resourceTwo = CommandResource.Create(Resource.From(Assets.Utilities.GetVideoFile()));
            var resourceList = new List<CommandResource>
                {
                    CommandResource.Create(Resource.From(Assets.Utilities.GetVideoFile())),
                    CommandResource.Create(Resource.From(Assets.Utilities.GetVideoFile()))
                }; 

            var commandResourceManager = CommandResourceManager.Create(command);

            CommandReceipt receipt = null; 
            Assert.DoesNotThrow(() => receipt = commandResourceManager.Add(resourceOne));
            Assert.DoesNotThrow(() => commandResourceManager.AddRange(resourceList));
            Assert.DoesNotThrow(() => commandResourceManager.Replace(receipt, resourceTwo));
            Assert.DoesNotThrow(() => commandResourceManager.Insert(0, resourceOne));
            Assert.True(command.Resources.Count == 4);
        }
    }
}
