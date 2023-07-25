using Velocity.Core.Models;

namespace Velocity.Core.Contracts.Services;

// Remove this class once your pages/features are using your data.
public interface ILogService
{
    IList<LogEntry> LogMessages
    {
        get;
    }
    void LogInformation(Exception ex, string message);

    void LogWarning(Exception ex, string message);

    void LogError(Exception ex, string message);

    void LogCritical(Exception ex, string message);

    void LogDebug(Exception ex, string message);

    void ClearLog();

    void OpenLog();
}
