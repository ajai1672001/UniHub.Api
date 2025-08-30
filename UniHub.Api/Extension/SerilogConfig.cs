using Serilog;
using Serilog.Filters;

namespace UniHub.Api.Extenstion
{
    public static class SerilogConfig
    {
        public static void LogConfig()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()

                // Write ONLY logs where LogType is null/empty → General
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(Matching.WithProperty<string>("LogType", v => !string.IsNullOrEmpty(v)))
                    .WriteTo.File(
                        "Logs/General/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 7
                    )
                )

                // Conditional per LogType (non-empty ones)
                .WriteTo.Map(
                    "LogType",
                    (logType, wt) =>
                    {
                        if (!string.IsNullOrEmpty(logType))
                        {
                            wt.File(
                                $"Logs/{logType}/log-.txt",
                                rollingInterval: RollingInterval.Day,
                                retainedFileCountLimit: 7
                            );
                        }
                    }
                )

                .CreateLogger();
        }
    }
}