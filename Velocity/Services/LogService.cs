﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using Velocity.Contracts.Services;
using Windows.Storage;
using Windows.System;
using Velocity.Helpers;
using Velocity.Models;

namespace Velocity.Services;
internal class LogService : ILogService
{
    private const string DefaultApplicationDataFolder = "Velocity\\ApplicationData";
    private const string DefaultLogFile = "VelocityLogs.json";

    private readonly string _localApplicationData =
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    private readonly string _applicationDataFolder;
    private readonly string _applicationLogFile;

    public LogService()
    {
        _applicationDataFolder = Path.Combine(_localApplicationData, DefaultApplicationDataFolder);
        _applicationLogFile = Path.Combine(_applicationDataFolder, DefaultLogFile);
    }

    public async Task InitializeAsync()
    {
        await SetupNLog();
    }

    public async Task SetupNLog()
    {
        var logFile = await GetFilePath();
        if (!File.Exists(logFile))
        {
            await using var fs = File.Create(logFile);
        }

        var config = new LoggingConfiguration();
        var jsonLayout = new JsonLayout
        {
            Attributes =
            {
                new JsonAttribute("timeStamp", "${date}"),
                new JsonAttribute("eventId", "${event-properties:EventId}"),
                new JsonAttribute("level", "${level}"),
                new JsonAttribute("logger", "${logger}"),
                new JsonAttribute("message", "${message}"),
                new JsonAttribute("exception", "${exception:format=toString,StackTrace}")
            },
            IncludeEventProperties = true
        };
        var logfile = new FileTarget("logfile") { FileName = logFile, Layout = jsonLayout };
        config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
        LogManager.Configuration = config;
    }

    public async Task OpenLogFolder()
    {
        var filePath = await GetFilePath();
        await FileExtension.OpenFile(filePath);
    }

    public Task<string> GetFilePath() => Task.FromResult(_applicationLogFile);
    public Task<string> GetFolderPath() => Task.FromResult(_applicationDataFolder);
}
