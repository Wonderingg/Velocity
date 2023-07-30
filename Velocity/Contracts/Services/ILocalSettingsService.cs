﻿using Windows.Devices.Display.Core;

namespace Velocity.Contracts.Services;

public interface ILocalSettingsService
{
    Task<T?> ReadSettingAsync<T>(string key);

    Task SaveSettingAsync<T>(string key, T value);

    Task<string> GetLogFolderAsync();

    void SetupNLog();
}
