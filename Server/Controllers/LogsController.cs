using static ThirdApp.Shared.Log;

namespace ThirdApp.Server;

[Route("api/[controller]")]
[ApiController]
public class LogsController : ControllerBase
{
    string logsAllocationPath = "App_Data\\Logs";
    List<Log> errorLogs = new();
    List<Log> warningLogs = new();
    List<Log> informationLogs = new();
    List<Log> criticalLogs = new();
    List<Log> debugLogs = new();

    public LogsController() => CheckLog();
    private void ReadErrorLogsFromFile()
    {
        if (System.IO.File.Exists($"{logsAllocationPath}\\errorLogs.json"))
        {
            string fileContent = System.IO.File.ReadAllText($"{logsAllocationPath}\\errorLogs.json");
            if (string.IsNullOrEmpty(fileContent)) return;

            errorLogs = JsonSerializer.Deserialize<List<Log>>(fileContent) ?? new List<Log>();
        }
    }
    private void ReadWarningLogsFromFile()
    {
        if (System.IO.File.Exists($"{logsAllocationPath}\\warningLogs.json"))
        {
            string fileContent = System.IO.File.ReadAllText($"{logsAllocationPath}\\warningLogs.json");
            if (string.IsNullOrEmpty(fileContent)) return;

            warningLogs = JsonSerializer.Deserialize<List<Log>>(fileContent) ?? new List<Log>();
        }
    }
    private void ReadInformationLogsFromFile()
    {
        if (System.IO.File.Exists($"{logsAllocationPath}\\informationLogs.json"))
        {
            string fileContent = System.IO.File.ReadAllText($"{logsAllocationPath}\\informationLogs.json");
            if (string.IsNullOrEmpty(fileContent)) return;

            informationLogs = JsonSerializer.Deserialize<List<Log>>(fileContent) ?? new List<Log>();
        }
    }
    private void ReadCriticalLogsFromFile()
    {
        if (System.IO.File.Exists($"{logsAllocationPath}\\criticalLogs.json"))
        {
            string fileContent = System.IO.File.ReadAllText($"{logsAllocationPath}\\criticalLogs.json");
            if (string.IsNullOrEmpty(fileContent)) return;

            criticalLogs = JsonSerializer.Deserialize<List<Log>>(fileContent) ?? new List<Log>();
        }
    }
    private void ReadDebugLogsFromFile()
    {
        if (System.IO.File.Exists($"{logsAllocationPath}\\debugLogs.json"))
        {
            string fileContent = System.IO.File.ReadAllText($"{logsAllocationPath}\\debugLogs.json");
            if (string.IsNullOrEmpty(fileContent)) return;

            debugLogs = JsonSerializer.Deserialize<List<Log>>(fileContent) ?? new List<Log>();
        }
    }
    private void CheckLog()
    {
        ReadErrorLogsFromFile();
        ReadWarningLogsFromFile();
        ReadInformationLogsFromFile();
        ReadCriticalLogsFromFile();
        ReadDebugLogsFromFile();
    }

    [HttpPost]
    public void CreateLog([FromBody] Log log)
    {
        if (log is null) throw new ArgumentNullException(nameof(log));

        log.Id = Guid.NewGuid();
        log.CreatedAt = DateTime.Now;

        if (string.IsNullOrEmpty(log.Message))
        {
            throw new ArgumentException("Message must not be empty.");
        }

        if (log.Type == LogType.Error)
        {
            errorLogs.Add(log);
            FlushErrorLogsToFile();
        }
        else if (log.Type == LogType.Warning)
        {
            warningLogs.Add(log);
            FlushWarningLogsToFile();
        }
        else if (log.Type == LogType.Information)
        {
            informationLogs.Add(log);
            FlushInformationLogsToFile();
        }
        else if (log.Type == LogType.Critical)
        {
            criticalLogs.Add(log);
            FlushCriticalLogsToFile();
        }
        else if (log.Type == LogType.Debug)
        {
            debugLogs.Add(log);
            FlushDebugLogsToFile();
        }
        else
        {
            throw new InvalidOperationException("Invalid log type");
        }
    }
    private void FlushErrorLogsToFile()
    {
        if (!Directory.Exists(logsAllocationPath))
            _ = Directory.CreateDirectory(logsAllocationPath);

        System.IO.File.WriteAllText($"{logsAllocationPath}\\errorLogs.json", JsonSerializer.Serialize(errorLogs));
    }

    private void FlushWarningLogsToFile()
    {
        if (!Directory.Exists(logsAllocationPath))
            _ = Directory.CreateDirectory(logsAllocationPath);

        System.IO.File.WriteAllText($"{logsAllocationPath}\\warningLogs.json", JsonSerializer.Serialize(warningLogs));
    }

    private void FlushInformationLogsToFile()
    {
        if (!Directory.Exists(logsAllocationPath))
            _ = Directory.CreateDirectory(logsAllocationPath);

        System.IO.File.WriteAllText($"{logsAllocationPath}\\informationLogs.json", JsonSerializer.Serialize(informationLogs));
    }

    private void FlushCriticalLogsToFile()
    {
        if (!Directory.Exists(logsAllocationPath))
            _ = Directory.CreateDirectory(logsAllocationPath);

        System.IO.File.WriteAllText($"{logsAllocationPath}\\criticalLogs.json", JsonSerializer.Serialize(criticalLogs));
    }

    private void FlushDebugLogsToFile()
    {
        DirectoryInfo? directoryInfo;
        if (!Directory.Exists(logsAllocationPath))
            directoryInfo = Directory.CreateDirectory(logsAllocationPath);

        System.IO.File.WriteAllText($"{logsAllocationPath}\\debugLogs.json", JsonSerializer.Serialize(debugLogs));
    }
    [HttpGet]
    public IEnumerable<Log> ReadAllLogs()
    {
        return ReadAllLogFiles();
    }

    private IEnumerable<Log> ReadAllLogFiles()
    {
        return errorLogs
                    .Concat(warningLogs)
                    .Concat(informationLogs)
                    .Concat(criticalLogs)
                    .Concat(debugLogs);
    }

    [HttpGet("{id}")]
    public string Get(int id) => "value";

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, [FromBody] Log updatedLog)
    {
        if (updatedLog == null)
        {
            return BadRequest("Log data is missing.");
        }

        bool updated = UpdateLog(id, updatedLog);
        if (!updated)
        {
            return NotFound("Log not found.");
        }

        return Ok("Log updated successfully.");
    }

    private bool UpdateLog(Guid id, Log updatedLog)
    {
        if (TryUpdateLogFromList(errorLogs, id, updatedLog, out bool updated))
        {
            FlushErrorLogsToFile();
        }
        else if (TryUpdateLogFromList(warningLogs, id, updatedLog, out updated))
        {
            FlushWarningLogsToFile();
        }
        else if (TryUpdateLogFromList(informationLogs, id, updatedLog, out updated))
        {
            FlushInformationLogsToFile();
        }
        else if (TryUpdateLogFromList(criticalLogs, id, updatedLog, out updated))
        {
            FlushCriticalLogsToFile();
        }
        else if (TryUpdateLogFromList(debugLogs, id, updatedLog, out updated))
        {
            FlushDebugLogsToFile();
        }

        return updated;
    }

    private static bool TryUpdateLogFromList(List<Log> logList, Guid id, Log updatedLog, out bool updated)
    {
        updated = false;

        Log? logToUpdate = logList.FirstOrDefault(log => log.Id == id);

        if (logToUpdate != null)
        {
            if (updatedLog.Type.HasValue)
            {
                logToUpdate.Type = updatedLog.Type.Value;
            }
            if (!string.IsNullOrEmpty(updatedLog.Message))
            {
                logToUpdate.Message = updatedLog.Message;
            }

            updated = true;
        }

        return updated;
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        bool deleted = DeleteLog(id);

        if (!deleted)
        {
            return NotFound("Log not found.");
        }

        return Ok("Log deleted successfully.");
    }

    private bool DeleteLog(Guid id)
    {

        if (TryDeleteLogFromList(errorLogs, id, out bool deleted))
        {
            FlushErrorLogsToFile();
        }
        else if (TryDeleteLogFromList(warningLogs, id, out deleted))
        {
            FlushWarningLogsToFile();
        }
        else if (TryDeleteLogFromList(informationLogs, id, out deleted))
        {
            FlushInformationLogsToFile();
        }
        else if (TryDeleteLogFromList(criticalLogs, id, out deleted))
        {
            FlushCriticalLogsToFile();
        }
        else if (TryDeleteLogFromList(debugLogs, id, out deleted))
        {
            FlushDebugLogsToFile();
        }

        return deleted;
    }

    private static bool TryDeleteLogFromList(List<Log> logList, Guid id, out bool deleted)
    {
        deleted = false;

        Log? logToDelete = logList.FirstOrDefault(log => log.Id == id);

        if (logToDelete != null)
        {
            logList.Remove(logToDelete);
            deleted = true;
        }

        return deleted;
    }
}