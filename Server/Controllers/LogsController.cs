using static ThirdApp.Shared.Log;

namespace ThirdApp.Server;

[Route("api/[controller]")]
[ApiController]
public class LogsController : ControllerBase
{
    string logsAllocationPath = "App_Data\\Logs";
    List<Log> errorLogs = new List<Log>();
    List<Log> warningLogs = new List<Log>();
    List<Log> informationLogs = new List<Log>();
    List<Log> criticalLogs = new List<Log>();
    List<Log> debugLogs = new List<Log>();

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
        DirectoryInfo? directoryInfo;
        if (!Directory.Exists(logsAllocationPath))
            directoryInfo = Directory.CreateDirectory(logsAllocationPath);

        System.IO.File.WriteAllText($"{logsAllocationPath}\\errorLogs.json", JsonSerializer.Serialize(errorLogs));
    }

    private void FlushWarningLogsToFile()
    {
        DirectoryInfo? directoryInfo;
        if (!Directory.Exists(logsAllocationPath))
            directoryInfo = Directory.CreateDirectory(logsAllocationPath);

        System.IO.File.WriteAllText($"{logsAllocationPath}\\warningLogs.json", JsonSerializer.Serialize(warningLogs));
    }

    private void FlushInformationLogsToFile()
    {
        DirectoryInfo? directoryInfo;
        if (!Directory.Exists(logsAllocationPath))
            directoryInfo = Directory.CreateDirectory(logsAllocationPath);

        System.IO.File.WriteAllText($"{logsAllocationPath}\\informationLogs.json", JsonSerializer.Serialize(informationLogs));
    }

    private void FlushCriticalLogsToFile()
    {
        DirectoryInfo? directoryInfo;
        if (!Directory.Exists(logsAllocationPath))
            directoryInfo = Directory.CreateDirectory(logsAllocationPath);

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
        IEnumerable<Log> allLogs = errorLogs
            .Concat(warningLogs)
            .Concat(informationLogs)
            .Concat(criticalLogs)
            .Concat(debugLogs);

        return allLogs;
    }

    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Log value)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}