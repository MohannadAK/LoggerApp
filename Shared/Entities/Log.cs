using System.Text.Json.Serialization;

namespace ThirdApp.Shared;

public class Log
{
    public DateTime CreatedAt { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LogType Type { get; set; }
    public string? Message { get; set; }
    public enum LogType
    {
        Error,
        Warning,
        Information,
        Critical,
        Debug
    }
}