using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace Hudl.FFmpeg.Logging
{
    public class LogUtility
    {
        private readonly ILog _log; 
 
        private LogUtility(Type loggerType)
        {
            _log = LogManager.GetLogger(loggerType);
            LogAttributes = new Dictionary<string, string>(); 
               
        }

        private Dictionary<string, string> LogAttributes { get; set; }

        public void SetAttributes(Dictionary<string, string> mergeDictionary)
        {
            if (mergeDictionary == null || mergeDictionary.Count == 0)
            {
                LogAttributes = new Dictionary<string, string>();
                return;
            }

            LogAttributes = mergeDictionary;
        }

        public void Debug(string message)
        {
            _log.Debug(GetLogMessage(message));
        }
        public void DebugFormat(string message, params object[] args)
        {
            _log.Debug(GetLogMessage(string.Format(message, args)));
        }

        public void Info(string message)
        {
            _log.Info(GetLogMessage(message));
        }
        public void InfoFormat(string message, params object[] args)
        {
            _log.Info(GetLogMessage(string.Format(message, args)));
        }

        public void Warn(string message)
        {
            _log.Warn(GetLogMessage(message));
        }
        public void WarnFormat(string message, params object[] args)
        {
            _log.Warn(GetLogMessage(string.Format(message, args)));
        }

        public void Error(string message)
        {
            _log.Error(GetLogMessage(message));
        }
        public void Error(string message, Exception e)
        {
            _log.Error(GetLogMessage(message), e);
        }
        public void ErrorFormat(string message, params object[] args)
        {
            _log.Error(GetLogMessage(string.Format(message, args)));
        }
        
        private Dictionary<string, string> GetLogMessage(string message)
        {
            var attributes = JoinDictionary(LogAttributes); 
            return new Dictionary<string, string> {
                    {"Program", "Hudl.FFmpeg"},
                    {"Message", message},
                    {"Attributes", string.Format("[{0}]", attributes)},
                };
        }

        public static LogUtility GetLogger(Type loggerType)
        {
            return new LogUtility(loggerType);
        }

        private static string JoinDictionary(Dictionary<string, string> dict)
        {
            return String.Join(",", dict.Select(e => String.Format("{0}={1}", e.Key, e.Value)).ToArray());
        }
    }
}
