using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Velocity.Core.Contracts.Services;
using Velocity.Core.Models;

namespace Velocity.ViewModels;

public class UpdateDetailViewModel : ObservableRecipient
{
    private readonly IWindowsUpdateService _windowsUpdateService;
    public ICommand DownloadAndInstallCommand
    {
        get;
    }
    public WindowsUpdate WindowsUpdate
    {
        get; set;
    }  
    public UpdateDetailViewModel(IWindowsUpdateService windowsUpdateService)
    {
        _windowsUpdateService = windowsUpdateService;
        DownloadAndInstallCommand = new RelayCommand<WindowsUpdate>(DownloadAndInstallUpdateAsync);
    }

    private async void DownloadAndInstallUpdateAsync(WindowsUpdate? update)
    {
        await _windowsUpdateService.DownloadUpdateAsync(update);
        await _windowsUpdateService.InstallUpdateAsync(update);
    }
}