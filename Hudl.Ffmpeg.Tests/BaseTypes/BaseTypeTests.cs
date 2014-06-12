using Hudl.Ffmpeg.BaseTypes;
using Hudl.Ffmpeg.Common;
using Hudl.Ffmpeg.Filters;
using Hudl.Ffmpeg.Filters.BaseTypes;
using Hudl.Ffmpeg.Resources;
using Xunit;

namespace Hudl.Ffmpeg.Tests.BaseTypes
{
    public class BaseTypeTests
    {
        [Fact]
        public void AppliesToCollection_Verify()
        {
            var appliesToCollection = new AppliesToCollection<IFilter>(typeof (Mp4)); 

            Assert.DoesNotThrow(() => appliesToCollection.Add(new Trim()));
            Assert.DoesNotThrow(() => appliesToCollection.Add(new SetDar()));

            Assert.Throws<AppliesToInvalidException>(() => appliesToCollection.Add(new AMix()));

            Assert.True(appliesToCollection.Count == 2);

            Assert.True(appliesToCollection.IndexOf<SetDar>() == 1);

            appliesToCollection.Merge(new SetDar(), FfmpegMergeOptionType.NewWins);
            appliesToCollection.Merge(new Overlay(), FfmpegMergeOptionType.OldWins); 

            Assert.True(appliesToCollection.Count == 3);
        }

    }
}
