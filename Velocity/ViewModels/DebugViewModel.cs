using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using Velocity.Contracts.Services;
using Velocity.Contracts.ViewModels;
using Velocity.Core.Contracts.Services;
using Velocity.Core.Models;
using Velocity.Core.Services;

namespace Velocity.ViewModels;

public partial class DebugViewModel : ObservableRecipient, INavigationAware
{
    private readonly ILogService _logService;

    public ObservableCollection<LogEntry> Logs
    {
        get;
    } = new();

    public DebugViewModel(ILogService logService)
    {
        _logService = logService;
    }

    public Task OnNavigatedTo(object parameter)
    {
        Logs.Clear();
        if (_logService.LogMessages != null)
        {
            foreach (var item in _logService.LogMessages)
            {
                Logs.Add(item);
            }
        }

        return Task.CompletedTask;
    }

    public void OnNavigatedFrom()
    {
    }

    public void ClearLog()
    {
        _logService.ClearLog();
        Logs.Clear();
    }

    public void OpenLog()   
    {
        _logService.OpenLog();
    }
}
