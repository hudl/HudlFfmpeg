using Hudl.FFmpeg.Command;
using System;

namespace Hudl.FFmpeg
{
    //a place to load the configuration file for the ffmpeg session.
    public class ResourceManagement
    {
        public static CommandConfiguration CommandConfiguration { get; set; }

      
        private static bool? _isMonoRuntime;
        /// <summary>
        /// This usually is considered poor programming style to need to have dependent processing based on runtime, however
        /// to implement timeout logic and work around existing issues with mono it is necessary. 
        /// 
        /// workaround(s)
        /// 1. workaround for a bug in the mono process when attempting to read async from console output events 
        ///      - link http://mono.1490590.n4.nabble.com/System-Diagnostic-Process-and-event-handlers-td3246096.html
        /// </summary>
        public static bool IsMonoRuntime()
        {
            if (!_isMonoRuntime.HasValue)
            {
                _isMonoRuntime = (Type.GetType("Mono.Runtime") != null);
            }

            return _isMonoRuntime ?? false;
        }
    }
}
