using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using NLog;
using Velocity.Contracts.Services;
using Velocity.Contracts.ViewModels;
using Velocity.Core.Contracts.Services;
using Velocity.Core.Models;
using Velocity.Core.Services;
using Velocity.Helpers;
using Velocity.Models;
using Velocity.Services;

namespace Velocity.ViewModels;

public partial class DebugViewModel : ObservableRecipient, INavigationAware
{
    public ObservableCollection<LogEvent> Logs
    {
        get;
    } = new();

    string _fileContents;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly ILocalSettingsService _localSettingsService;
    public DebugViewModel(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public Task OnNavigatedTo(object parameter)
    {
        LoadLogs();
        return Task.CompletedTask;
    }



    public void OnNavigatedFrom()
    {
        Logs.Clear();
    }

    private async Task<List<LogEvent>?> ParseLogFileAsync(string filePath)
    {
        var logs = new List<LogEvent>();
        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var streamReader = new StreamReader(fileStream);
        string? line;
        while ((line = await streamReader.ReadLineAsync()) != null)
        {
            try
            {
                var log = JsonConvert.DeserializeObject<LogEvent>(line);
                if (log != null)
                {
                    logs.Add(log);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to deserialize line in log file. Exception: {ex.Message}");
            }
        }

        if (!logs.Any())
        {
            Debug.WriteLine("No logs found in log file");
            return null;
        }

        return logs;
    }

    private async Task LoadLogs()
    {
        var filePath = await _localSettingsService.GetLogFolderAsync();
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.WriteLine("Log file path is null or empty");
            return;
        }

        var logs = await ParseLogFileAsync(filePath);
        if (logs != null)
        {
            foreach (var log in logs)
            {
                Logs.Add(log);
            }
        }
        else
        {
            Debug.WriteLine($"Failed to parse log file at {filePath}");
        }
    }

    public Task FixLogs()
    {
        _localSettingsService.SetupNLog();
        return Task.CompletedTask;
    }

    public async Task RefreshLogs()
    {
        await ClearLogs();
        await LoadLogs();
    }

    public Task ClearLogs()
    {
        Logs.Clear();
        return Task.CompletedTask;
    }

    public async Task GenerateSampleError()
    {
        try
        {
            int zero = 0;
            int result = 5 / zero;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Sample error");
            await RefreshLogs();
        }
    }
}
