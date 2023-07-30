using NLog;
using Velocity.Contracts.Services;
using Velocity.Models;
using Velocity.Services;

namespace Velocity.Helpers;

public class LogExtension
{
    public static Task Log(Logger logger, LogLevel logLevel, string message, LogEvent.EventID eventId)
    {
        var logEvent = new LogEventInfo(logLevel, logger.Name, message)
        {
            Properties =
            {
                ["EventId"] = eventId
            }
        };
        logger.Log(logEvent);
        return Task.CompletedTask;
    }
}