using Serilog;
using UniHub.Domain.Interface;

namespace UniHub.Service.Services
{
    public class TimeService : ITimeService
    {
        private readonly ILogger _emailLogger;
        private readonly ILogger _appleLogger;

        public TimeService()
        {
            _emailLogger = Log.ForContext("LogType", "EmailLog"); // writes to Logs/EmailLog-.log
            _appleLogger = Log.ForContext("LogType", "AppleLog"); // writes to Logs/AppleLog-.log
        }

        public DateTime GetCurrentTime()
        {
            _emailLogger.Information("📧 Email sent to {Recipient}", "Ajith");
            _appleLogger.Information("🍎 Apple service executed at {Time}", DateTime.UtcNow);
            throw new ApplicationException("error");
            return DateTime.UtcNow;
        }
    }
}