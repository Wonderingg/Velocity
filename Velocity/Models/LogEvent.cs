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
    public required string EventId
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
}
