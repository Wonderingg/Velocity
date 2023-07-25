#nullable enable
namespace Velocity.Core.Models;
public class LogEntry
{    
    public DateTime TimeStamp
    {
        get; set;
    }
    public string? LogLevel
    {
        get;
        set;
    }

    public string? EventId
    {
        get;
        set;
    }

    public string? Message
    {
        get;
        set;
    }

    public string? Exception
    {
        get;
        set;
    }
}