using Hudl.FFmpeg.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hudl.FFmpeg.Tests.Exceptions
{
    public class ExceptionTests
    {
        [Fact]
        public async Task ExceptionsRaised()
        {
            await Assert.ThrowsAsync<FFmpegProcessingException>(() => {
                throw new FFmpegProcessingException(1, "foo"); 
            });

            await Assert.ThrowsAsync<FFmpegRenderingException>(() => {
                throw new FFmpegRenderingException(null);
            });

            await Assert.ThrowsAsync<ForStreamInvalidException>(() => {
                throw new ForStreamInvalidException(typeof(string) , typeof(string));
            });

            await Assert.ThrowsAsync<StreamNotFoundException>(() => {
                throw new StreamNotFoundException(typeof(string));
            });

            await Assert.ThrowsAsync<StreamNotFoundException>(() => {
                throw new StreamNotFoundException();
            });
        }
    }
}
