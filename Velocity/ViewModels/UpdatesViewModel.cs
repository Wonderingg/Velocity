using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Velocity.Contracts.ViewModels;
using Velocity.Core.Contracts.Services;
using Velocity.Core.Models;

namespace Velocity.ViewModels;

public partial class UpdatesViewModel : ObservableRecipient, INavigationAware
{
    private readonly IWindowsUpdateService _windowsUpdateService;

    [ObservableProperty]
    private WindowsUpdate? _selected;

    public ObservableCollection<WindowsUpdate> AvailableUpdates
    {
        get;
        set;
    } = new();

    public UpdatesViewModel(IWindowsUpdateService windowsUpdateService)
    {
        _windowsUpdateService = windowsUpdateService;
    }

    public async Task OnNavigatedTo(object parameter)
    {
        await LoadUpdatesAsync();
    }

    public async Task LoadUpdatesAsync()
    {
        AvailableUpdates.Clear();

        var updates = await _windowsUpdateService.GetAvailableUpdatesAsync();

        foreach (var update in updates)
        {
            AvailableUpdates.Add(update);
        }
    }

    public async void DownloadAndInstallUpdate(WindowsUpdate update)
    {
        await _windowsUpdateService.DownloadUpdateAsync(update);
        await _windowsUpdateService.InstallUpdateAsync(update);
    }


    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        if (AvailableUpdates.Any())
        {
            Selected ??= AvailableUpdates.First();
        }
    }
}
