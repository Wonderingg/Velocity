using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Velocity.Contracts.ViewModels;
using Velocity.Core.Contracts.Services;
using Velocity.Core.Models;

namespace Velocity.ViewModels;

public partial class UpdatesViewModel : ObservableRecipient, INavigationAware
{
    private readonly IWindowsUpdateService _windowsUpdateService;
    private readonly WindowsUpdate _windowsUpdate;

    private UpdateDetailViewModel _selected;

    public UpdateDetailViewModel Selected
    {
        get => _selected;
        set
        {
            if (_selected == value)
            {
                return;
            }

            _selected = value;
            OnPropertyChanged(nameof(Selected));
        }
    }

    public ObservableCollection<UpdateDetailViewModel> AvailableUpdateViewModels { get; } = new();


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
        AvailableUpdateViewModels.Clear();

        var updates = await _windowsUpdateService.GetAvailableUpdatesAsync();

        foreach (var update in updates)
        {
            AvailableUpdateViewModels.Add(new UpdateDetailViewModel(_windowsUpdateService, _windowsUpdate) { WindowsUpdate = update });
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        if (AvailableUpdateViewModels.Any())
        {
            Selected ??= AvailableUpdateViewModels.First();
        }
    }
}
