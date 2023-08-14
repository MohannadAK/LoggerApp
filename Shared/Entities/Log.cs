namespace ThirdApp.Shared;

public class Log
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LogType? Type { get; set; }
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