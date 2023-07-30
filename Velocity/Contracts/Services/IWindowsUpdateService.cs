using Velocity.Core.Models;

namespace Velocity.Core.Contracts.Services;

public interface IWindowsUpdateService
{
    Task<IEnumerable<WindowsUpdate>> GetAvailableUpdatesAsync();
    Task DownloadUpdateAsync(WindowsUpdate update);
    Task InstallUpdateAsync(WindowsUpdate update);
}
