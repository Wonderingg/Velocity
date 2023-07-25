using System.Diagnostics;
using Newtonsoft.Json;
using Velocity.Core.Contracts.Services;
using Velocity.Core.Models;

namespace Velocity.Core.Services;

public class LogService : ILogService
{

    private const string DefaultApplicationDataFolder = "Velocity/ApplicationData";

    private readonly string _defaultAppDataFolder =
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    public IList<LogEntry> LogMessages
    {
        get; private set;
    }


    private string ApplicationDataFolder
    {
        get; set;
    }

    private string LogFile
    {
        get; set;
    }


    public LogService()
    {
        InitializeLogFile();
    }

    private void InitializeLogFile()
    {

        ApplicationDataFolder = Path.Combine(_defaultAppDataFolder, DefaultApplicationDataFolder);   
        LogFile = Path.Combine(ApplicationDataFolder, "Logs.json");
        if (!File.Exists(LogFile))
        {
            File.Create(LogFile).Dispose();
        }
    }

    private IList<LogEntry> ReadLogFile()
    {
        if (!File.Exists(LogFile))
        {
            return new List<LogEntry>();
        }
        var fileContent = File.ReadAllText(LogFile);
        return JsonConvert.DeserializeObject<List<LogEntry>>(fileContent) ?? new List<LogEntry>();
    }

    private void UpdateLogMessages()
    {
        LogMessages = ReadLogFile();
    }

    private void Log(string logLevel, Exception ex, string message)
    {
        var logEntry = new LogEntry()
        {
            TimeStamp = DateTime.Now,
            LogLevel = logLevel,
            EventId = "",
            Message = message,
            Exception = ex?.ToString()
        };

        WriteLogEntryToFile(logEntry);
    }

    private void WriteLogEntryToFile(LogEntry logEntry)
    {
        var existingContent = File.ReadAllText(LogFile);
        var existingEntries = JsonConvert.DeserializeObject<List<LogEntry>>(existingContent) ?? new List<LogEntry>();
        existingEntries.Add(logEntry);
        var updatedContent = JsonConvert.SerializeObject(existingEntries);
        File.WriteAllText(LogFile, updatedContent);
        UpdateLogMessages(); 
    }

    public void LogInformation(Exception ex, string message) => Log("Information", ex, message);
    public void LogWarning(Exception ex, string message) => Log("Warning", ex, message);
    public void LogError(Exception ex, string message) => Log("Error", ex, message);
    public void LogCritical(Exception ex, string message) => Log("Critical", ex, message);
    public void LogDebug(Exception ex, string message) => Log("Debug", ex, message);

    public void ClearLog()  
    {
        LogMessages.Clear();
        if (File.Exists(LogFile))
        {
            File.WriteAllText(LogFile, string.Empty);
        }
    }

    public void OpenLog()
    {
        if (File.Exists(LogFile))
        {
            Process.Start(new ProcessStartInfo(LogFile) { UseShellExecute = true });
        }
    }
}