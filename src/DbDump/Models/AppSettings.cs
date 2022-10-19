namespace DbDump.Models;

public record AppSettings
{
    private string _outputDirectory = string.Empty;

    public List<ConnectionSettings> Connections { get; set; } = new List<ConnectionSettings>();
    public string OutputDirectory
    {
        get => _outputDirectory;
        set => _outputDirectory = string.IsNullOrEmpty(value) ? "./database" : value;
    }
    public PostgresSettings Postgres { get; set; } = new PostgresSettings();
}

public record ConnectionSettings
{
    public string Database { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}

public record PostgresSettings
{
    public string PgDump { get; set; } = string.Empty;
}