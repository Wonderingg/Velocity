using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Velocity.Models;
public class LogEvent
{
    [JsonProperty("timeStamp")]
    public required string TimeStamp
    {
        get; set;
    }

    [JsonProperty("eventId")]
    public required EventID EventId
    {
        get; set;
    }

    [JsonProperty("logger")]
    public required string Logger
    {
        get; set;
    }

    [JsonProperty("level")]
    public required string Level
    {
        get; set;
    }

    [JsonProperty("message")]
    public required string Message
    {
        get; set;
    }

    [JsonProperty("exception")]
    public required string Exception
    {
        get; set;
    }

    public enum EventID
    {
        // General events
        Startup = 1,
        Shutdown = 2,
        OperationStarted = 3,
        OperationCompleted = 4,

        // WindowsUpdateService specific events
        AvailableUpdatesRetrieved = 10,
        AvailableUpdatesFailedToRetrieve = 11,
        UpdateDownloadStarted = 12,
        UpdateDownloadCompleted = 13,
        UpdateDownloadFailed = 14,
        UpdateInstallStarted = 15,
        UpdateInstallCompleted = 16,
        UpdateInstallFailed = 17,

        // Other service specific events
        ServiceStarted = 20,
        ServiceCompleted = 21,
        ServiceFailed = 22,

        // Error events
        GeneralError = 30,
        DatabaseError = 31,
        NetworkError = 32,
        IOError = 33,

        // Debug events
        DebugInformation = 50
    }
}
