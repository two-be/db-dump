using System.Diagnostics;
using DbDump.Models;
using DbDump.Utilities;
using FluentScheduler;
using Microsoft.Extensions.Configuration;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug().WriteTo.Console()
    .WriteTo.File($"logs/.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Hello, world!");

    IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();

    var settings = config.Get<AppSettings>();

    if (!Directory.Exists(settings.OutputDirectory))
    {
        Directory.CreateDirectory(settings.OutputDirectory);
    }

    JobManager.Initialize();

    JobManager.AddJob(
        () =>
        {
            settings.Connections.ForEach(x =>
            {
                try
                {
                    Log.Information(x.Database);
                    if (x.Type == ConnectionType.Postgres)
                    {
                        var outputFile = Path.Combine(settings.OutputDirectory, $"{x.Database}-{DateTime.Now:yyyy-MM-dd}.sql");
                        Process.Start("cmd", $@"/c ""{settings.Postgres.PgDump}"" --dbname=postgresql://{x.Username}:{x.Password}@{x.Host}:5432/{x.Database} --file {outputFile} --format=c --encoding UTF8");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("{ex}", ex);
                }
            });
        },
        s => s.NonReentrant().ToRunNow().AndEvery(1).Days().At(2, 2)
    );

    Console.ReadKey();

    Console.WriteLine("Hello, World!");
}
catch (Exception ex)
{
    Log.Error("{ex}", ex);
}