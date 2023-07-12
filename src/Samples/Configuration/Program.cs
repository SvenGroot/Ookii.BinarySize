using Configuration;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var settings = config.Get<Settings>();
Console.WriteLine($"The Size setting is {settings!.Size:0.# SiB}");
Console.WriteLine($"The IecSize setting is {settings!.IecSize:0.# SiB}");
