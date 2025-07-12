using NLog;

namespace LibraryManagementSystem.Api.Extensions
{
    public class LoggerHandler
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public string TraceId;

        public void Info(string message)
        {
            _logger.Info($"TRACEID: {TraceId}\n{message}");
        }
        public void Warn(string message)
        {
            _logger.Warn($"TRACEID: {TraceId}\n{message}");
        }
        public void Error(string message)
        {
            _logger.Error($"TRACEID: {TraceId}\n{message}");
        }
        public void Fatal(string message)
        {
            _logger.Fatal($"TRACEID: {TraceId}\n{message}");
        }
        public void Debug(string message)
        {
            _logger.Debug($"TRACEID: {TraceId}\n{message}");
        }
    }
}
