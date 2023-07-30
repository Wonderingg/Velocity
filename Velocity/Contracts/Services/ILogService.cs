using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velocity.Contracts.Services;
public interface ILogService
{
    Task OpenLogFolder();
    Task SetupNLog();
    Task<string> GetFilePath();
    Task<string> GetFolderPath();
}
