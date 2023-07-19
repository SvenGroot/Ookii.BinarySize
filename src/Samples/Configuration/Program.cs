using Configuration;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Read the configuration into the Settings class.
var settings = config.Get<Settings>();

// Show the two size settings, both using IEC unit formatting, with the shortest possible
// representation (the largest possible unit, using fractional values where necessary).
Console.WriteLine($"The Size setting is {settings!.Size:0.# SiB}");
Console.WriteLine($"The IecSize setting is {settings!.IecSize:0.# SiB}");
