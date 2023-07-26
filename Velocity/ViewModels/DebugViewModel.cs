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

    private readonly ILocalSettingsService _localSettingsService;

    public DebugViewModel(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task OnNavigatedTo(object parameter)
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

    public void OnNavigatedFrom()
    {
        Logs.Clear();
    }

    private async Task<List<LogEvent>?> ParseLogFileAsync(string filePath)
    {
        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var streamReader = new StreamReader(fileStream);
        _fileContents = await streamReader.ReadToEndAsync().ConfigureAwait(false);

        if (string.IsNullOrEmpty(_fileContents))
        {
            Debug.WriteLine("Log file contents are null or empty");
            return null;
        }

        List<LogEvent>? logs = null;
        try
        {
            logs = JsonConvert.DeserializeObject<List<LogEvent>>(_fileContents);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to deserialize log file contents. Exception: {ex.Message}");
        }

        if (logs == null || logs.Count == 0)
        {
            Debug.WriteLine("No logs found in log file");
        }

        return logs;
    }
}
