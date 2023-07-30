using System.Diagnostics;
using NLog;
using Velocity.Core.Contracts.Services;
using Velocity.Core.Models;
using Velocity;
using Velocity.Core.Helpers;
using Velocity.Models;
using NLog.Fluent;
using Velocity.Helpers;

namespace Velocity.Core.Services;
public class WindowsUpdateService : IWindowsUpdateService
{
    private readonly dynamic _updateSession = null;
    private readonly dynamic _updateSearcher = null;
    private dynamic _searchResult = null;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public WindowsUpdateService()
    {
#pragma warning disable CA1416
        _updateSession = Activator.CreateInstance(Type.GetTypeFromProgID("Microsoft.Update.Session") ?? throw new InvalidOperationException());
#pragma warning restore CA1416
        if (_updateSession == null)
        {
            return;
        }

        _updateSession.ClientApplicationID = "Velocity";
        _updateSearcher = _updateSession.CreateUpdateSearcher();
    }

    public async Task<IEnumerable<WindowsUpdate>> GetAvailableUpdatesAsync()
    {
        try
        {
            var updates = new List<WindowsUpdate>();
            await Task.Run(() =>
            {
                _searchResult = _updateSearcher.Search("IsInstalled=0");
                if (_searchResult == null)
                {
                    return;
                }

                for (var i = 0; i < _searchResult.Updates.Count; ++i)
                {
                    var update = _searchResult.Updates.Item(i);
                    updates.Add(new WindowsUpdate
                    {
                        Title = update.Title,
                        Description = update.Description,
                        IsDownloaded = update.IsDownloaded,
                        IsInstalled = update.IsInstalled
                    });
                }
            });
            await LogExtension.Log(Logger, LogLevel.Info, $"Successfully retrieved available Windows updates.", LogEvent.EventID.AvailableUpdatesRetrieved);
            return updates;
        }
        catch (Exception ex)
        {
            await LogExtension.Log(Logger, LogLevel.Error, ex.ToString(), LogEvent.EventID.AvailableUpdatesFailedToRetrieve);
            throw;
        }

    }

    public async Task DownloadUpdateAsync(WindowsUpdate update)
    {
        await Task.Run(() =>
        {
            LogExtension.Log(Logger, LogLevel.Info, $"Downloading {update.Title}", LogEvent.EventID.UpdateDownloadStarted);

#pragma warning disable CA1416
            dynamic updatesToDownload = Activator.CreateInstance(Type.GetTypeFromProgID("Microsoft.Update.UpdateColl") ?? throw new InvalidOperationException());
#pragma warning restore CA1416
            if (updatesToDownload != null)
            {
                updatesToDownload.Add(update);
                var downloader = _updateSession.CreateUpdateDownloader();
                downloader.Updates = updatesToDownload;
                var downloadResult = downloader.Download();

                if (downloadResult.ResultCode != 2)
                {
                    LogExtension.Log(Logger, LogLevel.Error, $"Download failed with result code: {downloadResult.ResultCode}", LogEvent.EventID.UpdateDownloadFailed);
                }
                LogExtension.Log(Logger, LogLevel.Info, $"Successfully downloaded {update.Title} with result code: {downloadResult.ResultCode}", LogEvent.EventID.UpdateDownloadCompleted);
            }
        });
    }

    public async Task InstallUpdateAsync(WindowsUpdate update)
    {
        await Task.Run(() =>
        {
            LogExtension.Log(Logger, LogLevel.Info, $"Installing {update.Title}", LogEvent.EventID.UpdateInstallStarted);
#pragma warning disable CA1416
            dynamic updatesToInstall = Activator.CreateInstance(Type.GetTypeFromProgID("Microsoft.Update.UpdateColl") ?? throw new InvalidOperationException());
#pragma warning restore CA1416
            if (updatesToInstall != null)
            {
                updatesToInstall.Add(update);
                var installer = _updateSession.CreateUpdateInstaller();
                installer.Updates = updatesToInstall;
                var installResult = installer.Install();

                if (installResult.ResultCode != 2)
                {
                    LogExtension.Log(Logger, LogLevel.Error, $"Install failed with result code: {installResult.ResultCode}", LogEvent.EventID.UpdateInstallFailed);
                }
                LogExtension.Log(Logger, LogLevel.Info, $"Successfully installed {update.Title} with result code: {installResult.ResultCode}", LogEvent.EventID.UpdateInstallCompleted);

            }
        });
    }
}