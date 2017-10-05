using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Collections;
using Hudl.FFmpeg.Common;
using Hudl.FFmpeg.Enums;
using Hudl.FFmpeg.Exceptions;
using Hudl.FFmpeg.Filters;
using Hudl.FFmpeg.Filters.Interfaces;
using Hudl.FFmpeg.Resources.BaseTypes;
using Xunit;

namespace Hudl.FFmpeg.Tests.BaseTypes
{
    public class BaseTypeTests
    {
        [Fact]
        public void AppliesToCollection_Verify()
        {
            var appliesToCollection = new ForStreamCollection<IFilter>(typeof (VideoStream)); 

            appliesToCollection.Add(new Trim());
            appliesToCollection.Add(new SetDar());

            Assert.Throws<ForStreamInvalidException>(() => appliesToCollection.Add(new AMix()));

            Assert.True(appliesToCollection.Count == 2);

            Assert.True(appliesToCollection.IndexOf<SetDar>() == 1);

            appliesToCollection.Merge(new SetDar(), FFmpegMergeOptionType.NewWins);
            appliesToCollection.Merge(new Overlay(), FFmpegMergeOptionType.OldWins); 

            Assert.True(appliesToCollection.Count == 3);
        }

        [Fact]
        public void ContainsStreamAttribute_Verify()
        {
            Assert.True(Validate.ContainsStream(typeof(ContainsStreamTestType), typeof(VideoStream)));

            Assert.False(Validate.ContainsStream(typeof(ContainsStreamTestType), typeof(AudioStream)));
        }

        [Fact]
        public void ForStreamAttribute_Verify()
        {
            Assert.True(AttributeValidation.AttributeTypeEquals(typeof(ForStreamTestType), typeof(VideoStream)));

            Assert.False(AttributeValidation.AttributeTypeEquals(typeof(ForStreamTestType), typeof(AudioStream)));
        }

        [ContainsStream(Type = typeof(VideoStream))]
        private class ContainsStreamTestType
        {
        }

        [ForStream(Type = typeof(VideoStream))]
        private class ForStreamTestType
        {
        }
    }
}
